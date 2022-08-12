﻿namespace Uroskur.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private const int MaxLocations = 100;
    private const int ExpireInHours = 1;
    private readonly IOpenWeatherClient _openWeatherClient;
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaService _stravaService;
    private readonly IYrClient _yrClient;

    public WeatherForecastService(IOpenWeatherClient openWeatherClient, IYrClient yrclient, IStravaService stravaService, IPreferencesService preferencesService)
    {
        _openWeatherClient = openWeatherClient;
        _yrClient = yrclient;
        _stravaService = stravaService;
        _preferencesService = preferencesService;
    }

    public async Task<IEnumerable<WeatherForecast>> FindOpenWeatherWeatherForecastsAsync(string? routeId, string? athleteId)
    {
        return await FindForecastsAsync(WeatherForecastProvider.OpenWeather, routeId, athleteId);
    }

    public async Task<IEnumerable<WeatherForecast>> FindYrWeatherForecastsAsync(string? routeId, string? athleteId)
    {
        return await FindForecastsAsync(WeatherForecastProvider.Yr, routeId, athleteId);
    }

    private async Task<IEnumerable<WeatherForecast>> FindForecastsAsync(WeatherForecastProvider weatherForecastProvider, string? routeId, string? athleteId)
    {
        Barrel.Current.EmptyExpired();

        if (string.IsNullOrEmpty(routeId))
        {
            Debug.WriteLine($"Route ID: {routeId} is invalid.");
            return Array.Empty<WeatherForecast>();
        }

        if (string.IsNullOrEmpty(athleteId))
        {
            Debug.WriteLine($"AthleteId ID: {athleteId} is invalid.");
            return Array.Empty<WeatherForecast>();
        }

        var forecasts = new List<WeatherForecast>();

        try
        {
            var locations = (await _stravaService.FindLocationsByAthleteIdRouteIdAsync(athleteId, routeId)).ToImmutableArray();
            if (locations == null)
            {
                throw new ArgumentException("Locations are null.");
            }

            if (locations.Length > MaxLocations)
            {
                throw new ArgumentException("Number of locations exceed maximum.");
            }

            var preference = _preferencesService.FindPreferences();
            var appId = preference.OpenWeatherAppId;
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentException("App ID is invalid.");
            }

            foreach (var location in locations)
            {
                var hourlyForecasts = new List<HourlyWeatherForecast>();

                if (weatherForecastProvider == WeatherForecastProvider.OpenWeather)
                {
                    var key = CacheKey(WeatherForecastProvider.OpenWeather, location);
                    OpenWeatherForecast? openWeatherForecast;

                    if (Barrel.Current.IsExpired(key))
                    {
                        openWeatherForecast = await _openWeatherClient.FetchForecastAsync(location, appId);
                        if (openWeatherForecast == null)
                        {
                            continue;
                        }

                        CacheForecast(key, openWeatherForecast.ToJson());
                    }
                    else
                    {
                        openWeatherForecast = OpenWeatherForecast.FromJson(FetchCachedForecast(key));
                    }

                    foreach (var hourly in openWeatherForecast.Hourly)
                    {
                        hourlyForecasts.Add(new HourlyWeatherForecast
                        {
                            Dt = DateTimeHelper.UnixTimestampToDateTime(hourly.Dt),
                            UnixTimestamp = hourly.Dt,
                            Temp = hourly.Temp,
                            FeelsLike = hourly.FeelsLike,
                            Uvi = hourly.Uvi,
                            Cloudiness = hourly.Clouds,
                            WindSpeed = hourly.WindSpeed,
                            WindGust = hourly.WindGust,
                            WindDeg = hourly.WindDeg,
                            Pop = hourly.Pop,
                            Icon = OpenWeatherIconsDictionary[hourly.Weather[0].Id]
                        });
                    }
                }
                else if (weatherForecastProvider == WeatherForecastProvider.Yr)
                {
                    var key = CacheKey(WeatherForecastProvider.Yr, location);
                    YrForecast? yrForecast;

                    if (Barrel.Current.IsExpired(key))
                    {
                        yrForecast = await _yrClient.FetchForecastAsync(location);
                        if (yrForecast == null)
                        {
                            continue;
                        }

                        CacheForecast(key, yrForecast.ToJson());
                    }
                    else
                    {
                        yrForecast = YrForecast.FromJson(FetchCachedForecast(key));
                    }

                    foreach (var timesery in yrForecast.Properties.Timeseries)
                    {
                        hourlyForecasts.Add(new HourlyWeatherForecast
                        {
                            Dt = timesery.Time.LocalDateTime,
                            UnixTimestamp = DateTimeHelper.DateTimeToUnixTimestamp(timesery.Time.LocalDateTime),
                            Temp = timesery.Data.Instant.Details.ContainsKey("air_temperature") ? timesery.Data.Instant.Details["air_temperature"] : 0D,
                            FeelsLike = timesery.Data.Instant.Details.ContainsKey("air_temperature") ? timesery.Data.Instant.Details["air_temperature"] : 0D,
                            Uvi = timesery.Data.Instant.Details.ContainsKey("ultraviolet_index_clear_sky") ? timesery.Data.Instant.Details["ultraviolet_index_clear_sky"] : 0D,
                            Cloudiness = timesery.Data.Instant.Details.ContainsKey("cloud_area_fraction") ? timesery.Data.Instant.Details["cloud_area_fraction"] : 0D,
                            WindSpeed = timesery.Data.Instant.Details.ContainsKey("wind_speed") ? timesery.Data.Instant.Details["wind_speed"] : 0D,
                            WindGust = timesery.Data.Instant.Details.ContainsKey("wind_speed_of_gust") ? timesery.Data.Instant.Details["wind_speed_of_gust"] : 0D,
                            WindDeg = timesery.Data.Instant.Details.ContainsKey("wind_from_direction") ? timesery.Data.Instant.Details["wind_from_direction"] : 0D,
                            Pop = Pop(timesery),
                            Icon = Icon(timesery)
                        });
                    }
                }
                else
                {
                    return Array.Empty<WeatherForecast>();
                }

                forecasts.Add(new WeatherForecast
                {
                    HourlyForecasts = hourlyForecasts
                });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Find forecasts failed {0} {1}", ex.Message, ex.StackTrace);
        }

        return forecasts;
    }

    private static void CacheForecast(string key, string json)
    {
        Debug.WriteLine($"Cache forecast with key: {key}.");
        Barrel.Current.Add(key, json, TimeSpan.FromHours(ExpireInHours));
    }

    private static string FetchCachedForecast(string key)
    {
        Debug.WriteLine($"Fetch cached forecast with key: {key}.");
        return Barrel.Current.Get<string>(key);
    }

    private static string CacheKey(WeatherForecastProvider provider, Location location)
    {
        var key = $"{provider}{location.Lat.ToString(CultureInfo.InvariantCulture)}{location.Lon.ToString(CultureInfo.InvariantCulture)}";
        Debug.WriteLine($"Cache key: {key}.");
        return key;
    }

    private static double Pop(Timesery timesery)
    {
        if (timesery.Data.Next1_Hours != null && timesery.Data.Next6_Hours != null)
        {
            return timesery.Data.Next1_Hours != null
                ? timesery.Data.Next1_Hours.Details.ProbabilityOfPrecipitation
                : timesery.Data.Next6_Hours.Details.ProbabilityOfPrecipitation;
        }

        return 0D;
    }

    private static string Icon(Timesery timesery)
    {
        var symbolCode = timesery.Data.Next6_Hours?.Summary.SymbolCode;
        if (symbolCode != null && YrIconsDictionary.ContainsKey(symbolCode))
        {
            return timesery.Data.Next6_Hours != null ? YrIconsDictionary[symbolCode.ToLower()] : "";
        }

        return string.Empty;
    }
}
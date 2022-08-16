namespace Uroskur.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private const int MaxLocations = 100;
#if DEBUG
    private const int ExpireInHours = 24;
#else
    private const int ExpireInHours = 1;
#endif
    private readonly IWeatherForecastClient _weatherForecastClient;
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaService _stravaService;

    public WeatherForecastService(IWeatherForecastClient weatherForecastClient, IStravaService stravaService, IPreferencesService preferencesService)
    {
        _weatherForecastClient = weatherForecastClient;
        _stravaService = stravaService;
        _preferencesService = preferencesService;
    }

    public async Task<IEnumerable<WeatherForecast>> FindWeatherForecastsAsync(
        WeatherForecastProvider weatherForecastProvider, string? routeId, string? athleteId)
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

        var weatherForecasts = new List<WeatherForecast>();

        try
        {
            var locations = (await _stravaService.FindLocationsByAthleteIdRouteIdAsync(athleteId, routeId))
                .ToImmutableArray();
            if (locations == null)
            {
                throw new ArgumentException("Locations are null.");
            }

            if (locations.Length > MaxLocations)
            {
                throw new ArgumentException("Number of locations exceed maximum allowed.");
            }

            var preference = _preferencesService.FindPreferences();
            var appId = preference.OpenWeatherAppId;
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentException("App ID is invalid.");
            }

            foreach (var location in locations)
            {
                var hourlyWeatherForecasts = new List<HourlyWeatherForecast>();

                if (weatherForecastProvider == OpenWeather)
                {
                    var key = CacheKey(OpenWeather, location);
                    OpenWeatherForecast? openWeatherWeatherForecast;

                    if (Barrel.Current.IsExpired(key))
                    {
                        openWeatherWeatherForecast =
                            await _weatherForecastClient.FetchOpenWeatherWeatherForecastAsync(location, appId);
                        if (openWeatherWeatherForecast == null)
                        {
                            continue;
                        }

                        CacheWeatherForecast(key, openWeatherWeatherForecast.ToJson());
                    }
                    else
                    {
                        openWeatherWeatherForecast = OpenWeatherForecast.FromJson(FetchCachedWeatherForecast(key));
                    }

                    foreach (var hourly in openWeatherWeatherForecast.Hourly)
                    {
                        hourlyWeatherForecasts.Add(new HourlyWeatherForecast
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
                            Pop = Math.Round(hourly.Pop * 100),
                            PrecipitationAmount = hourly.Rain?.The1H ?? 0D,
                            Icon = OpenWeatherIconsDictionary[hourly.Weather[0].Id]
                        });
                    }
                }
                else if (weatherForecastProvider == Yr)
                {
                    var key = CacheKey(Yr, location);
                    YrForecast? yrWeatherForecast;

                    if (Barrel.Current.IsExpired(key))
                    {
                        yrWeatherForecast = await _weatherForecastClient.FetchYrWeatherForecastAsync(location);
                        if (yrWeatherForecast == null)
                        {
                            continue;
                        }

                        CacheWeatherForecast(key, yrWeatherForecast.ToJson());
                    }
                    else
                    {
                        yrWeatherForecast = YrForecast.FromJson(FetchCachedWeatherForecast(key));
                    }

                    foreach (var timesery in yrWeatherForecast.Properties.Timeseries)
                    {
                        hourlyWeatherForecasts.Add(new HourlyWeatherForecast
                        {
                            Dt = timesery.Time.LocalDateTime,
                            UnixTimestamp = DateTimeHelper.DateTimeToUnixTimestamp(timesery.Time.LocalDateTime),
                            Temp = timesery.Data.Instant.Details.ContainsKey("air_temperature")
                                ? timesery.Data.Instant.Details["air_temperature"]
                                : 0D,
                            FeelsLike = timesery.Data.Instant.Details.ContainsKey("air_temperature")
                                ? timesery.Data.Instant.Details["air_temperature"]
                                : 0D,
                            Uvi = timesery.Data.Instant.Details.ContainsKey("ultraviolet_index_clear_sky")
                                ? timesery.Data.Instant.Details["ultraviolet_index_clear_sky"]
                                : 0D,
                            Cloudiness = timesery.Data.Instant.Details.ContainsKey("cloud_area_fraction")
                                ? timesery.Data.Instant.Details["cloud_area_fraction"]
                                : 0D,
                            WindSpeed = timesery.Data.Instant.Details.ContainsKey("wind_speed")
                                ? timesery.Data.Instant.Details["wind_speed"]
                                : 0D,
                            WindGust = timesery.Data.Instant.Details.ContainsKey("wind_speed_of_gust")
                                ? timesery.Data.Instant.Details["wind_speed_of_gust"]
                                : 0D,
                            WindDeg = timesery.Data.Instant.Details.ContainsKey("wind_from_direction")
                                ? timesery.Data.Instant.Details["wind_from_direction"]
                                : 0D,
                            PrecipitationAmount = PrecipitationAmount(timesery),
                            Pop = Pop(timesery),
                            Icon = Icon(timesery)
                        });
                    }
                }
                else if (weatherForecastProvider == Smhi)
                {
                    var key = CacheKey(Smhi, location);
                    SmhiForecast? smhiWeatherForecast;

                    if (Barrel.Current.IsExpired(key))
                    {
                        smhiWeatherForecast = await _weatherForecastClient.FetchSmhiWeatherForecastAsync(location);
                        if (smhiWeatherForecast == null)
                        {
                            continue;
                        }

                        CacheWeatherForecast(key, smhiWeatherForecast.ToJson());
                    }
                    else
                    {
                        smhiWeatherForecast = SmhiForecast.FromJson(FetchCachedWeatherForecast(key));
                    }

                    foreach (var timesery in smhiWeatherForecast.TimeSeries)
                    {
                        var dt = timesery.ValidTime.GetValueOrDefault().LocalDateTime;
                        var unixTimestamp = DateTimeHelper.DateTimeToUnixTimestamp(dt);

                        foreach (var parameter in timesery.Parameters)
                        {
                            var temp = 0D;
                            var windSpeed = 0D;
                            var windGust = 0D;
                            var windDeg = 0D;
                            var precipitationAmount = 0D;
                            var icon = string.Empty;

                            switch (parameter.Name)
                            {
                                // Temp (Air temperature)
                                case T:
                                    temp = parameter.Values.FirstOrDefault();
                                    break;
                                // WindSpeed (Wind speed)
                                case Ws:
                                    windSpeed = parameter.Values.FirstOrDefault();
                                    break;
                                // WindGust (Wind gust speed)
                                case Gust:
                                    windGust = parameter.Values.FirstOrDefault();
                                    break;
                                // WindDeg (Wind direction)
                                case Wd:
                                    windDeg = parameter.Values.FirstOrDefault();
                                    break;
                                // PrecipitationAmount (Mean precipitation intensity)
                                case Pmean:
                                    precipitationAmount = parameter.Values.FirstOrDefault();
                                    break;
                                // Icon
                                case Wsymb2:
                                    icon = string.Empty;
                                    break;
                            }

                            hourlyWeatherForecasts.Add(new HourlyWeatherForecast
                            {
                                Dt = dt,
                                UnixTimestamp = unixTimestamp,
                                Temp = temp,
                                FeelsLike = 0D, // Missing
                                Uvi = 0D, // Missing
                                Cloudiness = 0D, // Missing
                                WindSpeed = windSpeed,
                                WindGust = windGust,
                                WindDeg = windDeg,
                                PrecipitationAmount = precipitationAmount,
                                Pop = 0D, // Missing
                                Icon = icon
                            });
                        }
                    }
                }
                else
                {
                    return Array.Empty<WeatherForecast>();
                }

                weatherForecasts.Add(new WeatherForecast
                {
                    HourlyWeatherForecasts = hourlyWeatherForecasts
                });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Find forecasts failed {0} {1}", ex.Message, ex.StackTrace);
        }

        return weatherForecasts;
    }

    private static void CacheWeatherForecast(string key, string json)
    {
        Debug.WriteLine($"Cache weather forecast with key: {key}.");
        Barrel.Current.Add(key, json, TimeSpan.FromHours(ExpireInHours));
    }

    private static string FetchCachedWeatherForecast(string key)
    {
        Debug.WriteLine($"Fetch weather cached forecast with key: {key}.");
        return Barrel.Current.Get<string>(key);
    }

    private static string CacheKey(WeatherForecastProvider provider, Location location)
    {
        var key =
            $"{provider}{location.Lat.ToString(CultureInfo.InvariantCulture)}{location.Lon.ToString(CultureInfo.InvariantCulture)}";
        Debug.WriteLine($"Cache key: {key}.");
        return key;
    }

    private static double Pop(Timesery timesery)
    {
        if (timesery.Data.Next1_Hours != null && timesery.Data.Next6_Hours != null)
        {
            return timesery.Data.Next1_Hours != null
                ? Math.Round(timesery.Data.Next1_Hours.Details.ProbabilityOfPrecipitation)
                : Math.Round(timesery.Data.Next6_Hours.Details.ProbabilityOfPrecipitation);
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

    private static double PrecipitationAmount(Timesery timesery)
    {
        if (timesery.Data.Next1_Hours != null && timesery.Data.Next6_Hours != null)
        {
            return timesery.Data.Next1_Hours != null
                ? timesery.Data.Next1_Hours.Details.PrecipitationAmount
                : timesery.Data.Next6_Hours.Details.PrecipitationAmount;
        }

        return 0D;
    }
}
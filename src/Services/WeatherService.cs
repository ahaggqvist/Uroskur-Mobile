namespace Uroskur.Services;

public class WeatherService : IWeatherService
{
    private const int HoursToExpire = 1;
    private readonly IOpenWeatherClient _openWeatherClient;
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaService _stravaService;
    private readonly IYrClient _yrClient;

    public WeatherService(IOpenWeatherClient openWeatherClient, IYrClient yrclient, IStravaService stravaService, IPreferencesService preferencesService)
    {
        _openWeatherClient = openWeatherClient;
        _yrClient = yrclient;
        _stravaService = stravaService;
        _preferencesService = preferencesService;
    }

    public async Task<IEnumerable<Forecast>> FindForecastsAsync(ForecastProvider forecastProvider, string? routeId, string? athleteId)
    {
        Barrel.Current.EmptyExpired();

        //Barrel.Current.EmptyAll();

        if (string.IsNullOrEmpty(routeId))
        {
            Debug.WriteLine($"Route ID: {routeId} is invalid.");

            return Array.Empty<Forecast>();
        }

        if (string.IsNullOrEmpty(athleteId))
        {
            Debug.WriteLine($"AthleteId ID: {athleteId} is invalid.");

            return Array.Empty<Forecast>();
        }

        var forecasts = new List<Forecast>();

        try
        {
            var locations = (await _stravaService.FindLocationsByAthleteIdRouteIdAsync(athleteId, routeId)).ToImmutableArray();
            foreach (var location in locations)
            {
                var hourlyForecasts = new List<HourlyForecast>();

                switch (forecastProvider)
                {
                    case ForecastProvider.OpenWeather:
                    {
                        var key =
                            $"OpenWeather{location.Lat.ToString(CultureInfo.InvariantCulture)}{location.Lon.ToString(CultureInfo.InvariantCulture)}";

                        Debug.WriteLine($"Is OpenWeather forecast cache expired: {Barrel.Current.IsExpired(key)}.");

                        OpenWeatherForecast? openWeatherForecast;

                        if (Barrel.Current.IsExpired(key))
                        {
                            var preference = _preferencesService.FindPreferences();
                            var appId = preference.OpenWeatherAppId;
                            openWeatherForecast = await _openWeatherClient.FetchForecastAsync(location, appId);
                            if (openWeatherForecast == null)
                            {
                                continue;
                            }

                            Barrel.Current.Add(key, openWeatherForecast.ToJson(), TimeSpan.FromHours(HoursToExpire));

                            Debug.WriteLine($"Cache forecast with key: {key}.");
                        }
                        else
                        {
                            var json = Barrel.Current.Get<string>(key);
                            openWeatherForecast = OpenWeatherForecast.FromJson(json);
                            if (openWeatherForecast == null)
                            {
                                continue;
                            }

                            Debug.WriteLine($"Fetch cached forecast with key: {key}.");
                        }

                        hourlyForecasts.AddRange(openWeatherForecast.Hourly.Select(hourly => new HourlyForecast
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
                            IconId = hourly.Weather[0].Id
                        }));
                        break;
                    }
                    case ForecastProvider.Yr:
                    {
                        var key =
                            $"Yr{location.Lat.ToString(CultureInfo.InvariantCulture)}{location.Lon.ToString(CultureInfo.InvariantCulture)}";

                        Debug.WriteLine($"Is Yr forecast cache expired: {Barrel.Current.IsExpired(key)}.");

                        YrForecast? yrForecast;

                        if (Barrel.Current.IsExpired(key))
                        {
                            yrForecast = await _yrClient.FetchForecastAsync(location);
                            if (yrForecast == null)
                            {
                                continue;
                            }

                            Barrel.Current.Add(key, yrForecast.ToJson(), TimeSpan.FromHours(HoursToExpire));

                            Debug.WriteLine($"Cache forecast with key: {key}.");
                        }
                        else
                        {
                            var json = Barrel.Current.Get<string>(key);
                            yrForecast = YrForecast.FromJson(json);
                            if (yrForecast == null)
                            {
                                continue;
                            }

                            Debug.WriteLine($"Fetch cached forecast with key: {key}.");
                        }

                        foreach (var timesery in yrForecast.Properties.Timeseries)
                        {
                            if (timesery == null)
                            {
                                continue;
                            }

                            var pop = 0D;
                            if (timesery.Data.Next1_Hours != null && timesery.Data.Next6_Hours != null)
                            {
                                pop = timesery.Data.Next1_Hours != null
                                    ? timesery.Data.Next1_Hours.Details.ProbabilityOfPrecipitation
                                    : timesery.Data.Next6_Hours.Details.ProbabilityOfPrecipitation;
                            }

                            var hourlyForecast = new HourlyForecast
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
                                Pop = pop,
                                IconId = 800
                            };

                            Debug.WriteLine(hourlyForecast.ToString());

                            hourlyForecasts.Add(hourlyForecast);
                        }
                    }
                        break;
                    default:
                        return Array.Empty<Forecast>();
                }

                forecasts.Add(new Forecast
                {
                    HourlyForecasts = hourlyForecasts
                });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FindForecastAsync {0} {1}", ex.Message, ex.StackTrace);
        }

        return forecasts;
    }
}
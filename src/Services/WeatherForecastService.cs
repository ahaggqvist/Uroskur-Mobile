namespace Uroskur.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private const int MaxLocations = 100;
#if DEBUG
    private const int ExpireInHours = 24;
#else
    private const int ExpireInHours = 1;
#endif
    private readonly AppSettings? _appSettings;
    private readonly IWeatherForecastClient _weatherForecastClient;
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaService _stravaService;

    public WeatherForecastService(AppSettings? appSettings, IWeatherForecastClient weatherForecastClient, IStravaService stravaService, IPreferencesService preferencesService)
    {
        _appSettings = appSettings;
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

            var weatherForecasts = new List<WeatherForecast>();
            foreach (var location in locations)
            {
                var hourlyWeatherForecasts = new List<HourlyWeatherForecast>();

                if (weatherForecastProvider == OpenWeather)
                {
                    var preference = _preferencesService.FindPreferences();
                    var appId = preference.OpenWeatherAppId;
                    if (string.IsNullOrEmpty(appId))
                    {
                        throw new ArgumentException("App ID is invalid.");
                    }

                    var url = _appSettings?.OpenWeatherApiUrl!;
                    var weatherForecastProviderData = await GetWeatherForecastProviderData(location, url, appId);
                    if (weatherForecastProviderData is { OpenWeatherData: { } })
                    {
                        GetOpenWeatherHourlyWeatherForecasts(weatherForecastProviderData.OpenWeatherData, hourlyWeatherForecasts);
                    }
                }
                else if (weatherForecastProvider == Yr)
                {
                    var url = _appSettings?.YrApiUrl!;
                    var weatherForecastProviderData = await GetWeatherForecastProviderData(location, url);
                    if (weatherForecastProviderData is { YrData: { } }) GetYrHourlyWeatherForecasts(weatherForecastProviderData.YrData, hourlyWeatherForecasts);
                }
                else if (weatherForecastProvider == Smhi)
                {
                    var url = _appSettings?.SmhiApiUrl!;
                    var weatherForecastProviderData = await GetWeatherForecastProviderData(location, url);
                    if (weatherForecastProviderData?.SmhiData?.TimeSeries != null)
                    {
                        GetSmhiHourlyForecasts(weatherForecastProviderData.SmhiData, hourlyWeatherForecasts);
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

            return weatherForecasts;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Find forecasts failed {0} {1}", ex.Message, ex.StackTrace);
        }

        return new List<WeatherForecast>();
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

    private async Task<WeatherForecastProviderData?> GetWeatherForecastProviderData(Location location, string url, string appId = "")
    {
        var weatherForecastProvider = WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(url);
        if (weatherForecastProvider == OpenWeather)
        {
            var key = CacheKey(OpenWeather, location);
            if (!Barrel.Current.IsExpired(key)) return new WeatherForecastProviderData(OpenWeatherData.FromJson(FetchCachedWeatherForecast(key)));
            var weatherForecastProviderData = await _weatherForecastClient.FetchWeatherForecastProviderDataAsync(url.Replace("@AppId", appId)
                .Replace("@Exclude", "current,minutely,daily,alerts")
                .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
                .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture)));

            if (weatherForecastProviderData == null)
            {
                Debug.WriteLine("OpenWeather data is null.");
                return null;
            }

            var weatherData = weatherForecastProviderData.OpenWeatherData;
            CacheWeatherForecast(key, weatherData.ToJson());

            return weatherForecastProviderData;
        }

        if (weatherForecastProvider == Yr)
        {
            var key = CacheKey(Yr, location);
            if (!Barrel.Current.IsExpired(key)) return new WeatherForecastProviderData(YrData.FromJson(FetchCachedWeatherForecast(key)));
            var weatherForecastProviderData = await _weatherForecastClient.FetchWeatherForecastProviderDataAsync(url
                .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
                .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture)));

            if (weatherForecastProviderData == null)
            {
                Debug.WriteLine("Yr data is null.");
                return null;
            }

            var weatherData = weatherForecastProviderData.YrData;
            CacheWeatherForecast(key, weatherData.ToJson());

            return weatherForecastProviderData;
        }

        if (weatherForecastProvider == Smhi)
        {
            var key = CacheKey(Smhi, location);
            if (!Barrel.Current.IsExpired(key)) return new WeatherForecastProviderData(SmhiData.FromJson(FetchCachedWeatherForecast(key)));
            var weatherForecastProviderData = await _weatherForecastClient.FetchWeatherForecastProviderDataAsync(url
                .Replace("@Lat",
                    Math.Round(location.Lat, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture))
                .Replace("@Lon",
                    Math.Round(location.Lon, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture)));

            if (weatherForecastProviderData == null)
            {
                Debug.WriteLine("Smhi data is null.");
                return null;
            }

            var weatherData = weatherForecastProviderData.SmhiData;
            CacheWeatherForecast(key, weatherData.ToJson());

            return weatherForecastProviderData;
        }

        return null;
    }

    private static void GetOpenWeatherHourlyWeatherForecasts(OpenWeatherData openWeatherData, ICollection<HourlyWeatherForecast> hourlyWeatherForecasts)
    {
        foreach (var hourly in openWeatherData.Hourly)
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

    private static void GetYrHourlyWeatherForecasts(YrData yrData, ICollection<HourlyWeatherForecast> hourlyWeatherForecasts)
    {
        foreach (var timesery in yrData.Properties.Timeseries)
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

    private static void GetSmhiHourlyForecasts(SmhiData smhiData, ICollection<HourlyWeatherForecast> hourlyWeatherForecasts)
    {
        foreach (var timesery in smhiData.TimeSeries)
        {
            var dt = timesery.ValidTime.GetValueOrDefault().LocalDateTime;
            var unixTimestamp = DateTimeHelper.DateTimeToUnixTimestamp(dt);
            var temp = 0D;
            var windSpeed = 0D;
            var windGust = 0D;
            var windDeg = 0D;
            var precipitationAmount = 0D;
            var icon = string.Empty;

            foreach (var parameter in timesery.Parameters)
            {
                // Temp (Air temperature)
                if (parameter.Name == T)
                {
                    temp = parameter.Values.FirstOrDefault();
                }
                // WindSpeed (Wind speed)
                else if (parameter.Name == Ws)
                {
                    windSpeed = parameter.Values.FirstOrDefault();
                }
                // WindGust (Wind gust speed)
                else if (parameter.Name == Gust)
                {
                    windGust = parameter.Values.FirstOrDefault();
                }
                // WindDeg (Wind direction)
                else if (parameter.Name == Wd)
                {
                    windDeg = parameter.Values.FirstOrDefault();
                }
                // PrecipitationAmount (Mean precipitation intensity)
                else if (parameter.Name == Pmean)
                {
                    precipitationAmount = parameter.Values.FirstOrDefault();
                }
                // Icon
                else if (parameter.Name == Wsymb2)
                {
                    icon = SmhiIconsDictionary[parameter.Values.FirstOrDefault()];
                }
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
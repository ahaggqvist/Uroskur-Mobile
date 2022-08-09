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

    public async Task<IEnumerable<Forecast>> FindForecastsAsync(string? routeId, string? athleteId)
    {
        Barrel.Current.EmptyExpired();

        var preference = _preferencesService.FindPreferences();
        var appId = preference.OpenWeatherAppId;

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
                var key =
                    $"{location.Lat.ToString(CultureInfo.InvariantCulture)}{location.Lon.ToString(CultureInfo.InvariantCulture)}";

                Debug.WriteLine($"Is forecast cache expired: {Barrel.Current.IsExpired(key)}.");

                OpenWeatherForecast? openWeatherForecast;

                if (Barrel.Current.IsExpired(key))
                {
                    openWeatherForecast = await _openWeatherClient.GetForecastAsync(location, appId);
                    if (openWeatherForecast == null)
                    {
                        continue;
                    }

                    Barrel.Current.Add(key, openWeatherForecast.ToJson(), TimeSpan.FromHours(HoursToExpire));

                    Debug.WriteLine($"Cache temperature with key: {key}.");
                }
                else
                {
                    var json = Barrel.Current.Get<string>(key);
                    openWeatherForecast = OpenWeatherForecast.FromJson(json);
                    if (openWeatherForecast == null)
                    {
                        continue;
                    }

                    Debug.WriteLine($"Fetch cached temperature with key: {key}.");
                }

                var hourlyForecasts = new List<HourlyForecast>();

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
namespace Uroskur.Services;

public class OpenWeatherService : IOpenWeatherService
{
    private const int HoursToExpire = 1;
    private readonly IOpenWeatherClient _openWeatherClient;
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaService _stravaService;

    public OpenWeatherService(IOpenWeatherClient openWeatherClient, IStravaService stravaService, IPreferencesService preferencesService)
    {
        _openWeatherClient = openWeatherClient;
        _stravaService = stravaService;
        _preferencesService = preferencesService;
    }

    public async Task<IEnumerable<Temperatures>> FindForecastAsync(string? routeId, string? athleteId)
    {
        Barrel.Current.EmptyExpired();

        var preference = _preferencesService.FindPreferences();
        var appId = preference.OpenWeatherAppId;

        if (string.IsNullOrEmpty(routeId))
        {
            Debug.WriteLine($"Route ID: {routeId} is invalid.");

            return Array.Empty<Temperatures>();
        }

        if (string.IsNullOrEmpty(athleteId))
        {
            Debug.WriteLine($"AthleteId ID: {athleteId} is invalid.");

            return Array.Empty<Temperatures>();
        }

        var temperatures = new List<Temperatures>();

        try
        {
            var locations = (await _stravaService.FindLocationsByAthleteIdRouteIdAsync(athleteId, routeId)).ToList();
            foreach (var location in locations)
            {
                var key =
                    $"{location.Lat.ToString(CultureInfo.InvariantCulture)}{location.Lon.ToString(CultureInfo.InvariantCulture)}";

                Debug.WriteLine($"Is forecast cache expired: {Barrel.Current.IsExpired(key)}.");

                if (Barrel.Current.IsExpired(key))
                {
                    var temperature = await _openWeatherClient.GetForecastAsync(location, appId);
                    if (temperature == null)
                    {
                        continue;
                    }

                    Barrel.Current.Add(key, temperature.ToJson(), TimeSpan.FromHours(HoursToExpire));

                    Debug.WriteLine($"Cache temperature with key: {key}.");

                    temperatures.Add(temperature);
                }
                else
                {
                    var json = Barrel.Current.Get<string>(key);
                    var temperature = Temperatures.FromJson(json);
                    if (temperature == null)
                    {
                        continue;
                    }

                    Debug.WriteLine($"Fetch cached temperature with key: {key}.");

                    temperatures.Add(temperature);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FindForecastAsync {0} {1}", ex.Message, ex.StackTrace);
        }

        return temperatures;
    }
}
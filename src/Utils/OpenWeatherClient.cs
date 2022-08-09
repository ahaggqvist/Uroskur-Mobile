namespace Uroskur.Utils;

public class OpenWeatherClient : IOpenWeatherClient
{
    private const int MaxLocations = 100;
    private const int MaxRetryAttempts = 3;
    private const int PauseBetweenFailures = 2;
    private readonly AppSettings? _appSettings;
    private readonly HttpClient? _httpClient;

    public OpenWeatherClient(AppSettings? appSettings, HttpClient? httpClient)
    {
        _appSettings = appSettings;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<OpenWeatherForecast>> FetchForecastsAsync(IEnumerable<Location>? locations, string? appId)
    {
        if (locations == null)
        {
            throw new ArgumentException("Locations are null.");
        }

        locations = locations.ToImmutableArray();
        if (locations.Count() > MaxLocations)
        {
            throw new ArgumentException("Locations exceed maximum.");
        }

        if (string.IsNullOrEmpty(appId))
        {
            throw new ArgumentException("App ID is invalid.");
        }

        var apiUrl = _appSettings?.OpenWeatherApiUrl;
        if (string.IsNullOrEmpty(apiUrl))
        {
            throw new ArgumentException("Forecast url is invalid.");
        }

        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var urls = locations.Select(location => apiUrl
                .Replace("@AppId", appId)
                .Replace("@Exclude", "current,minutely,daily,alerts")
                .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
                .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture)))
            .ToImmutableArray();

        var openWeatherForecasts = new List<OpenWeatherForecast>();
        await retryPolicy.ExecuteAsync(async () =>
        {
            var responses = urls.Select(GetResponseAsync).ToArray();
            Task.WhenAll(responses).Wait(60000);

            foreach (var response in responses)
            {
                if (OpenWeatherForecast.FromJson(await response) is { } t)
                {
                    openWeatherForecasts.Add(t);
                }
            }
        });

        return openWeatherForecasts;
    }

    public async Task<OpenWeatherForecast?> FetchForecastAsync(Location location, string? appId)
    {
        if (location == null)
        {
            throw new ArgumentException("Location is null.");
        }

        if (string.IsNullOrEmpty(appId))
        {
            throw new ArgumentException("App ID is invalid.");
        }

        var apiUrl = _appSettings?.OpenWeatherApiUrl;
        if (string.IsNullOrEmpty(apiUrl))
        {
            throw new ArgumentException("OpenWeather API url is invalid.");
        }

        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var url = apiUrl
            .Replace("@AppId", appId)
            .Replace("@Exclude", "current,minutely,daily,alerts")
            .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture));

        Debug.WriteLine($"API Url {url}");

        OpenWeatherForecast? openWeatherForecast = null;
        await retryPolicy.ExecuteAsync(async () =>
        {
            openWeatherForecast = OpenWeatherForecast.FromJson(await GetResponseAsync(url));
        });


        return openWeatherForecast;
    }

    private Task<string> GetResponseAsync(string? url)
    {
        return _httpClient?.GetStringAsync(url)!;
    }
}
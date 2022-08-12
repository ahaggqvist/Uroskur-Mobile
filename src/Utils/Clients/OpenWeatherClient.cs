namespace Uroskur.Utils.Clients;

public class OpenWeatherClient : IOpenWeatherClient
{
    private const int MaxRetryAttempts = 3;
    private const int PauseBetweenFailures = 2;
    private readonly AppSettings? _appSettings;
    private readonly HttpClient? _httpClient;

    public OpenWeatherClient(AppSettings? appSettings, HttpClient? httpClient)
    {
        _appSettings = appSettings;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<OpenWeatherForecast>> FetchWeatherForecastsAsync(IEnumerable<Location>? locations, string? appId)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var openWeatherApiUrl = _appSettings?.OpenWeatherApiUrl!;
        var urls = locations!.Select(location => openWeatherApiUrl
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
                if (OpenWeatherForecast.FromJson(await response) is { } openWeatherForecast)
                {
                    openWeatherForecasts.Add(openWeatherForecast);
                }
            }
        });

        return openWeatherForecasts;
    }

    public async Task<OpenWeatherForecast?> FetchWeatherForecastAsync(Location location, string? appId)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var openWeatherApiUrl = _appSettings?.OpenWeatherApiUrl!;
        var url = openWeatherApiUrl
            .Replace("@AppId", appId)
            .Replace("@Exclude", "current,minutely,daily,alerts")
            .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture));

        Debug.WriteLine($"OpenWeather API Url {url}");

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
namespace Uroskur.Utils.Clients;

public class YrClient : IYrClient
{
    private const string UserAgent = "Uroskur/1.0.0-alpha github.com/ahaggqvist/uroskur-maui";
    private const int MaxRetryAttempts = 3;
    private const int PauseBetweenFailures = 2;
    private readonly AppSettings? _appSettings;
    private readonly HttpClient? _httpClient;

    public YrClient(AppSettings? appSettings, HttpClient? httpClient)
    {
        _appSettings = appSettings;
        _httpClient = httpClient;
        _httpClient?.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
    }

    public async Task<IEnumerable<YrForecast>> FetchWeatherForecastsAsync(IEnumerable<Location>? locations)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var yrApiUrl = _appSettings?.YrApiUrl!;
        var urls = locations!.Select(location => yrApiUrl
                .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
                .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture)))
            .ToImmutableArray();

        var yrForecasts = new List<YrForecast>();
        await retryPolicy.ExecuteAsync(async () =>
        {
            var responses = urls.Select(GetResponseAsync).ToArray();
            Task.WhenAll(responses).Wait(60000);

            foreach (var response in responses)
            {
                if (YrForecast.FromJson(await response) is { } yrForecast)
                {
                    yrForecasts.Add(yrForecast);
                }
            }
        });

        return yrForecasts;
    }

    public async Task<YrForecast?> FetchWeatherForecastAsync(Location location)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var yrApiUrl = _appSettings?.YrApiUrl!;
        var url = yrApiUrl
            .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture));

        Debug.WriteLine($"Yr API Url {url}.");

        YrForecast? yrForecast = null;
        await retryPolicy.ExecuteAsync(async () => { yrForecast = YrForecast.FromJson(await GetResponseAsync(url)); });


        return yrForecast;
    }

    private Task<string> GetResponseAsync(string? url)
    {
        return _httpClient?.GetStringAsync(url)!;
    }
}
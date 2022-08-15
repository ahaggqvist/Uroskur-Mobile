namespace Uroskur.Utils.Clients;

public class SmhiClient : ISmhiClient
{
    private const string UserAgent = "Uroskur/1.0.0-alpha github.com/ahaggqvist/uroskur-maui";
    private const int MaxRetryAttempts = 3;
    private const int PauseBetweenFailures = 2;
    private readonly AppSettings? _appSettings;
    private readonly HttpClient? _httpClient;

    public SmhiClient(AppSettings? appSettings, HttpClient? httpClient)
    {
        _appSettings = appSettings;
        _httpClient = httpClient;
        _httpClient?.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
    }

    public async Task<IEnumerable<SmhiForecast>> FetchWeatherForecastsAsync(IEnumerable<Location>? locations)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var smhiApiUrl = _appSettings?.SmhiApiUrl!;
        var urls = locations!.Select(location => smhiApiUrl
                .Replace("@Lat",
                    Math.Round(location.Lat, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture))
                .Replace("@Lon",
                    Math.Round(location.Lon, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture)))
            .ToImmutableArray();

        var smhiForecasts = new List<SmhiForecast>();
        await retryPolicy.ExecuteAsync(async () =>
        {
            var responses = urls.Select(GetResponseAsync).ToArray();
            Task.WhenAll(responses).Wait(60000);

            foreach (var response in responses)
            {
                if (SmhiForecast.FromJson(await response) is { } yrForecast)
                {
                    smhiForecasts.Add(yrForecast);
                }
            }
        });

        return smhiForecasts;
    }

    public async Task<SmhiForecast?> FetchWeatherForecastAsync(Location location)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var smhiApiUrl = _appSettings?.SmhiApiUrl!;
        var url = smhiApiUrl
            .Replace("@Lat",
                Math.Round(location.Lat, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon",
                Math.Round(location.Lon, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture));

        Debug.WriteLine($"Smhi API Url {url}.");

        SmhiForecast? smhiForecast = null;
        await retryPolicy.ExecuteAsync(async () =>
        {
            smhiForecast = SmhiForecast.FromJson(await GetResponseAsync(url));
        });


        return smhiForecast;
    }

    private Task<string> GetResponseAsync(string? url)
    {
        return _httpClient?.GetStringAsync(url)!;
    }
}
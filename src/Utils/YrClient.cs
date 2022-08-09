namespace Uroskur.Utils;

public class YrClient : IYrClient
{
    private const string UserAgent = "Uroskur/1.0.0-alpha github.com/ahaggqvist/uroskur-maui";
    private const int MaxLocations = 100;
    private const int MaxRetryAttempts = 3;
    private const int PauseBetweenFailures = 2;
    private readonly AppSettings? _appSettings;
    private readonly HttpClient? _httpClient;

    public YrClient(AppSettings? appSettings, HttpClient? httpClient)
    {
        _appSettings = appSettings;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<YrForecast>> FetchForecastsAsync(IEnumerable<Location>? locations)
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

        var apiUrl = _appSettings?.YrApiUrl;
        if (string.IsNullOrEmpty(apiUrl))
        {
            throw new ArgumentException("Yr API url is invalid.");
        }

        _httpClient?.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);

        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var urls = locations.Select(location => apiUrl
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
                if (YrForecast.FromJson(await response) is { } t)
                {
                    yrForecasts.Add(t);
                }
            }
        });

        return yrForecasts;
    }

    public async Task<YrForecast?> FetchForecastAsync(Location location)
    {
        if (location == null)
        {
            throw new ArgumentException("Location is null.");
        }

        var apiUrl = _appSettings?.YrApiUrl;
        if (string.IsNullOrEmpty(apiUrl))
        {
            throw new ArgumentException("Forecast url is invalid.");
        }

        _httpClient?.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);

        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var url = apiUrl
            .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture));

        Debug.WriteLine($"API Url {url}");

        YrForecast? yrForecast = null;
        await retryPolicy.ExecuteAsync(async () =>
        {
            yrForecast = YrForecast.FromJson(await GetResponseAsync(url));
        });


        return yrForecast;
    }

    private Task<string> GetResponseAsync(string? url)
    {
        return _httpClient?.GetStringAsync(url)!;
    }
}
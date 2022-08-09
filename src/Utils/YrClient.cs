namespace Uroskur.Utils;

public class YrClient : IYrClient
{
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

    public async Task<IEnumerable<YrForecast>> GetForecastAsync(IEnumerable<Location>? locations, string? appId)
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

        var forecastUrl = _appSettings?.ForecastUrl;
        if (string.IsNullOrEmpty(forecastUrl))
        {
            throw new ArgumentException("Forecast url is invalid.");
        }

        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var urls = locations.Select(location => forecastUrl
                .Replace("@AppId", appId)
                .Replace("@Exclude", "current,minutely,daily,alerts")
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

    public async Task<YrForecast?> GetForecastAsync(Location location, string? appId)
    {
        if (location == null)
        {
            throw new ArgumentException("Location is null.");
        }

        if (string.IsNullOrEmpty(appId))
        {
            throw new ArgumentException("App ID is invalid.");
        }

        var forecastUrl = _appSettings?.ForecastUrl;
        if (string.IsNullOrEmpty(forecastUrl))
        {
            throw new ArgumentException("Forecast url is invalid.");
        }

        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        var url = forecastUrl
            .Replace("@AppId", appId)
            .Replace("@Exclude", "current,minutely,daily,alerts")
            .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture));

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
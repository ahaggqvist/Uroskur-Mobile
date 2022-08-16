namespace Uroskur.Utils.Clients;

public class WeatherForecastClient : IWeatherForecastClient
{
    private const string UserAgent = "Uroskur/1.0.0-alpha github.com/ahaggqvist/uroskur-maui";
    private const int MaxRetryAttempts = 3;
    private const int PauseBetweenFailures = 2;
    private readonly AppSettings? _appSettings;
    private readonly HttpClient? _httpClient;

    public WeatherForecastClient(AppSettings? appSettings, HttpClient? httpClient)
    {
        _appSettings = appSettings;
        _httpClient = httpClient;
        _httpClient?.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
    }

    public async Task<OpenWeatherForecast?> FetchOpenWeatherWeatherForecastAsync(Location location, string? appId)
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

    public async Task<SmhiForecast?> FetchSmhiWeatherForecastAsync(Location location)
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

    public async Task<YrForecast?> FetchYrWeatherForecastAsync(Location location)
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
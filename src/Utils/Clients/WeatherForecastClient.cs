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
        var url = _appSettings?.OpenWeatherApiUrl!
            .Replace("@AppId", appId)
            .Replace("@Exclude", "current,minutely,daily,alerts")
            .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture));

        var (openWeatherForecast, _, _) = await FetchForecastAsync(OpenWeather, url);
        return openWeatherForecast;
    }

    public async Task<SmhiForecast?> FetchSmhiWeatherForecastAsync(Location location)
    {
        var url = _appSettings?.SmhiApiUrl!
            .Replace("@Lat",
                Math.Round(location.Lat, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon",
                Math.Round(location.Lon, 6, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture));

        var (_, _, smhiForecast) = await FetchForecastAsync(Smhi, url);
        return smhiForecast;
    }

    public async Task<YrForecast?> FetchYrWeatherForecastAsync(Location location)
    {
        var url = _appSettings?.YrApiUrl!
            .Replace("@Lat", location.Lat.ToString(CultureInfo.InvariantCulture))
            .Replace("@Lon", location.Lon.ToString(CultureInfo.InvariantCulture));

        var (_, yrForecast, _) = await FetchForecastAsync(Yr, url);
        return yrForecast;
    }

    private async Task<(OpenWeatherForecast? openWeatherForecast, YrForecast? yrForecast, SmhiForecast? smhiForecast)> FetchForecastAsync(
        Enumeration provider, string? url)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        Debug.WriteLine($"API Url {url}");

        OpenWeatherForecast? openWeatherForecast = null;
        YrForecast? yrForecast = null;
        SmhiForecast? smhiForecast = null;
        await retryPolicy.ExecuteAsync(async () =>
        {
            if (provider == OpenWeather)
            {
                openWeatherForecast = OpenWeatherForecast.FromJson(await GetResponseAsync(url));
            }
            else if (provider == Yr)
            {
                yrForecast = YrForecast.FromJson(await GetResponseAsync(url));
            }
            else if (provider == Smhi)
            {
                smhiForecast = SmhiForecast.FromJson(await GetResponseAsync(url));
            }
        });

        return (openWeatherForecast, yrForecast, smhiForecast);
    }

    private Task<string> GetResponseAsync(string? url)
    {
        return _httpClient?.GetStringAsync(url)!;
    }
}
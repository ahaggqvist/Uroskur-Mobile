namespace Uroskur.Utils.Clients;

public class WeatherForecastClient : IWeatherForecastClient
{
    private const string UserAgent = "Uroskur/1.0.0-alpha github.com/ahaggqvist/uroskur-maui";
    private const int MaxRetryAttempts = 3;
    private const int PauseBetweenFailures = 2;
    private readonly HttpClient? _httpClient;

    public WeatherForecastClient(HttpClient? httpClient)
    {
        _httpClient = httpClient;
        _httpClient?.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
    }

    public async Task<OpenWeatherForecast?> FetchOpenWeatherWeatherForecastAsync(string url)
    {
        var (openWeatherForecast, _, _) = await FetchForecastAsync(OpenWeather, url);
        return openWeatherForecast;
    }

    public async Task<SmhiForecast?> FetchSmhiWeatherForecastAsync(string url)
    {
        var (_, _, smhiForecast) = await FetchForecastAsync(Smhi, url);
        return smhiForecast;
    }

    public async Task<YrForecast?> FetchYrWeatherForecastAsync(string url)
    {
        var (_, yrForecast, _) = await FetchForecastAsync(Yr, url);
        return yrForecast;
    }

    private async Task<(OpenWeatherForecast? openWeatherForecast, YrForecast? yrForecast, SmhiForecast? smhiForecast)> FetchForecastAsync(
        WeatherForecastProvider weatherForecastProvider, string? url)
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
            if (weatherForecastProvider == OpenWeather)
            {
                openWeatherForecast = OpenWeatherForecast.FromJson(await GetResponseAsync(url));
            }
            else if (weatherForecastProvider == Yr)
            {
                yrForecast = YrForecast.FromJson(await GetResponseAsync(url));
            }
            else if (weatherForecastProvider == Smhi)
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
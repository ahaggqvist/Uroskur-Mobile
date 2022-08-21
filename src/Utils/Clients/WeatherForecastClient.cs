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

    public async Task<WeatherForecastProviderData?> FetchWeatherForecastProviderDataAsync(string url)
    {
        var weatherForecastProvider = WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(url);
        if (weatherForecastProvider == OpenWeather)
        {
            return new WeatherForecastProviderData
            {
                OpenWeatherData = await FetchDataAsync(OpenWeatherData.FromJson, url)
            };
        }

        if (weatherForecastProvider == Yr)
        {
            return new WeatherForecastProviderData
            {
                YrData = await FetchDataAsync(YrData.FromJson, url)
            };
        }

        if (weatherForecastProvider == Smhi)
        {
            return new WeatherForecastProviderData
            {
                SmhiData = await FetchDataAsync(SmhiData.FromJson, url)
            };
        }

        throw new ArgumentException($"Weather forecast provider is unknown for url: {url}.");
    }

    private async Task<T?> FetchDataAsync<T>(Func<string, T> weatherForecastProvider, string? url) where T : class
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        Debug.WriteLine($"Url {url}");

        T? data = null;
        await retryPolicy.ExecuteAsync(async () =>
        {
            data = weatherForecastProvider(await GetResponseAsync(url));
        });

        return data;
    }

    private Task<string> GetResponseAsync(string? url)
    {
        return _httpClient?.GetStringAsync(url)!;
    }
}
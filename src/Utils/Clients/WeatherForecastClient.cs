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
            var (openWeatherData, _, _) = await FetchDataAsync(OpenWeather, url);
            if (openWeatherData != null) return new WeatherForecastProviderData(openWeatherData);
        }
        else if (weatherForecastProvider == Yr)
        {
            var (_, yrData, _) = await FetchDataAsync(Yr, url);
            if (yrData != null) return new WeatherForecastProviderData(yrData);
        }
        else if (weatherForecastProvider == Smhi)
        {
            var (_, _, smhiData) = await FetchDataAsync(Smhi, url);
            if (smhiData != null) return new WeatherForecastProviderData(smhiData);
        }

        throw new ArgumentException($"Weather forecast provider is unknown for url: {url}.");
    }

    private async Task<(OpenWeatherData? openWeatherData, YrData? yrData, SmhiData? smhiData)> FetchDataAsync(
        WeatherForecastProvider weatherForecastProvider, string? url)
    {
        var pauseBetweenFailures = TimeSpan.FromSeconds(PauseBetweenFailures);
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(MaxRetryAttempts, _ => pauseBetweenFailures);

        Debug.WriteLine($"API Url {url}");

        OpenWeatherData? openWeatherForecast = null;
        YrData? yrForecast = null;
        SmhiData? smhiForecast = null;
        await retryPolicy.ExecuteAsync(async () =>
        {
            if (weatherForecastProvider == OpenWeather)
            {
                openWeatherForecast = OpenWeatherData.FromJson(await GetResponseAsync(url));
            }
            else if (weatherForecastProvider == Yr)
            {
                yrForecast = YrData.FromJson(await GetResponseAsync(url));
            }
            else if (weatherForecastProvider == Smhi)
            {
                smhiForecast = SmhiData.FromJson(await GetResponseAsync(url));
            }
        });

        return (openWeatherForecast, yrForecast, smhiForecast);
    }

    private Task<string> GetResponseAsync(string? url)
    {
        return _httpClient?.GetStringAsync(url)!;
    }
}
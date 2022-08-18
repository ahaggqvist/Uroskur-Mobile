namespace Uroskur.Utils.Clients;

public interface IWeatherForecastClient
{
    Task<WeatherForecastProviderData?> FetchWeatherForecastProviderDataAsync(string url);
}
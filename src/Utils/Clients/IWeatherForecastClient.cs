namespace Uroskur.Utils.Clients;

public interface IWeatherForecastClient
{
    Task<OpenWeatherForecast?> FetchOpenWeatherWeatherForecastAsync(string url);
    Task<SmhiForecast?> FetchSmhiWeatherForecastAsync(string url);
    Task<YrForecast?> FetchYrWeatherForecastAsync(string url);
}
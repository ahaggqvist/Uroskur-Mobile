namespace Uroskur.Utils.Clients;

public interface IWeatherForecastClient
{
    Task<OpenWeatherForecast?> FetchOpenWeatherWeatherForecastAsync(Location location, string? appId);
    Task<SmhiForecast?> FetchSmhiWeatherForecastAsync(Location location);
    Task<YrForecast?> FetchYrWeatherForecastAsync(Location location);
}
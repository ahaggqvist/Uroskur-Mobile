namespace Uroskur.Utils.Clients;

public interface IOpenWeatherClient
{
    Task<IEnumerable<OpenWeatherForecast>> FetchWeatherForecastsAsync(IEnumerable<Location>? locations, string? appId);
    Task<OpenWeatherForecast?> FetchWeatherForecastAsync(Location location, string? appId);
}
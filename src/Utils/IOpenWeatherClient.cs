namespace Uroskur.Utils;

public interface IOpenWeatherClient
{
    Task<IEnumerable<OpenWeatherForecast>> GetForecastAsync(IEnumerable<Location>? locations, string? appId);
    Task<OpenWeatherForecast?> GetForecastAsync(Location location, string? appId);
}
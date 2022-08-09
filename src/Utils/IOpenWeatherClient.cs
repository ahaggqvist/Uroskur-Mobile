namespace Uroskur.Utils;

public interface IOpenWeatherClient
{
    Task<IEnumerable<OpenWeatherForecast>> FetchForecastsAsync(IEnumerable<Location>? locations, string? appId);
    Task<OpenWeatherForecast?> FetchForecastAsync(Location location, string? appId);
}
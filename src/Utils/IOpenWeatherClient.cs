namespace Uroskur.Utils;

public interface IOpenWeatherClient
{
    Task<List<Temperatures>> GetForecastAsync(IEnumerable<Location>? locations, string? appId);
    Task<Temperatures?> GetForecastAsync(Location location, string? appId);
}
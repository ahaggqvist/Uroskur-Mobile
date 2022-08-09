namespace Uroskur.Utils;

public interface IYrClient
{
    Task<IEnumerable<YrForecast>> GetForecastAsync(IEnumerable<Location>? locations, string? appId);
    Task<YrForecast?> GetForecastAsync(Location location, string? appId);
}
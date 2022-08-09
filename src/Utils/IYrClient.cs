namespace Uroskur.Utils;

public interface IYrClient
{
    Task<IEnumerable<YrForecast>> FetchForecastsAsync(IEnumerable<Location>? locations);
    Task<YrForecast?> FetchForecastAsync(Location location);
}
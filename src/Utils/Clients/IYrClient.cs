namespace Uroskur.Utils.Clients;

public interface IYrClient
{
    Task<IEnumerable<YrForecast>> FetchWeatherForecastsAsync(IEnumerable<Location>? locations);
    Task<YrForecast?> FetchWeatherForecastAsync(Location location);
}
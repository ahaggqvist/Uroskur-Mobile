namespace Uroskur.Utils.Clients;

public interface ISmhiClient
{
    Task<IEnumerable<SmhiForecast>> FetchWeatherForecastsAsync(IEnumerable<Location>? locations);
    Task<SmhiForecast?> FetchWeatherForecastAsync(Location location);
}
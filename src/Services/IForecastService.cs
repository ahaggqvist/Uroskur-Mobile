namespace Uroskur.Services;

public interface IForecastService
{
    Task<IEnumerable<Forecast>> FindOpenWeatherForecastsAsync(string? routeId, string? athleteId);

    Task<IEnumerable<Forecast>> FindYrForecastsAsync(string? routeId, string? athleteId);
}
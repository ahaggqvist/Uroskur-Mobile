namespace Uroskur.Services;

public interface IWeatherService
{
    Task<IEnumerable<Forecast>> FindForecastsAsync(string? routeId, string? athleteId);
}
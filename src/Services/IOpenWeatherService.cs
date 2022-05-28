namespace Uroskur.Services;

public interface IOpenWeatherService
{
    Task<IEnumerable<Temperatures>> FindForecastAsync(string? routeId, string? athleteId);
}
namespace Uroskur.Services;

public interface IWeatherService
{
    Task<IEnumerable<Temperatures>> FindForecastAsync(string? routeId, string? athleteId);
}
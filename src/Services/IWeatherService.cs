using Uroskur.Models.OpenWeather;

namespace Uroskur.Services;

public interface IWeatherService
{
    Task<IEnumerable<OpenWeatherForecast>> FindForecastAsync(string? routeId, string? athleteId);
}
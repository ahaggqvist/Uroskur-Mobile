namespace Uroskur.Services;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> FindOpenWeatherWeatherForecastsAsync(string? routeId, string? athleteId);

    Task<IEnumerable<WeatherForecast>> FindYrWeatherForecastsAsync(string? routeId, string? athleteId);
}
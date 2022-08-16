namespace Uroskur.Services;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> FindWeatherForecastsAsync(WeatherForecastProvider weatherForecastProvider, string? routeId, string? athleteId);
}
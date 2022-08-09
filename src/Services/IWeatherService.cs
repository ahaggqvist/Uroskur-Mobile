namespace Uroskur.Services;

public interface IWeatherService
{
    Task<IEnumerable<Forecast>> FindForecastsAsync(ForecastProvider forecastProvider, string? routeId, string? athleteId);
}
namespace Uroskur.Models;

public class WeatherForecast
{
    public IEnumerable<HourlyWeatherForecast> HourlyWeatherForecasts { get; set; } = new List<HourlyWeatherForecast>();
}
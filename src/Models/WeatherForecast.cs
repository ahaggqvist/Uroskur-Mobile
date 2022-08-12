namespace Uroskur.Models;

public class WeatherForecast
{
    public IEnumerable<HourlyWeatherForecast> HourlyForecasts { get; set; } = new List<HourlyWeatherForecast>();
}
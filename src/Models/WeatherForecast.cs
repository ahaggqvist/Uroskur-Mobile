namespace Uroskur.Models;

public record WeatherForecast(IEnumerable<HourlyWeatherForecast> HourlyWeatherForecasts)
{
    public DateTime? SunriseToday { get; set; }

    public DateTime? SunsetToday { get; set; }

    public DateTime? SunriseTomorrow { get; set; }

    public DateTime? SunsetTomorrow { get; set; }
}
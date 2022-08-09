namespace Uroskur.Models;

public class Forecast
{
    public IEnumerable<HourlyForecast> HourlyForecasts { get; set; } = new List<HourlyForecast>();
}
namespace Uroskur.Models;

public class LocationForecast
{
    public int Km { get; set; }

    public DateTime Dt { get; set; }

    public HourlyForecast HourlyForecast { get; set; } = new();

    public string WeatherIcon { get; set; } = string.Empty;

    public string WindIcon { get; set; } = string.Empty;

    public string WindIconId { get; set; } = string.Empty;

    public string TimeKmFormatted => $"{Dt:HH:mm}";
}
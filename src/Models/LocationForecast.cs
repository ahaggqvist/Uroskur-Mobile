using Uroskur.Models.OpenWeather;

namespace Uroskur.Models;

public class LocationForecast
{
    public int Km { get; set; }

    public DateTime UnixDateTime { get; set; }

    public Hourly? Hourly { get; set; }

    public string? WeatherIcon { get; set; }

    public string? WindIcon { get; set; }

    public string? WindIconId { get; set; }

    public string TimeKmFormatted => $"{UnixDateTime:HH:mm}";
}
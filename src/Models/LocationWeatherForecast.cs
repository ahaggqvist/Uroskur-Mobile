namespace Uroskur.Models;

public class LocationWeatherForecast
{
    public int Km { get; set; }

    public DateTime Dt { get; set; }

    public HourlyWeatherForecast HourlyWeatherForecast { get; set; } = new();

    public string WeatherIcon { get; set; } = string.Empty;

    public string WindIcon { get; set; } = string.Empty;

    public string WindIconId { get; set; } = string.Empty;

    public string TimeKmFormatted => $"{Dt:HH:mm}";

    public override string ToString()
    {
        return
            $"{nameof(Km)}: {Km}, {nameof(Dt)}: {Dt}, {nameof(HourlyWeatherForecast)}: {HourlyWeatherForecast}, {nameof(WeatherIcon)}: {WeatherIcon}, {nameof(WindIcon)}: {WindIcon}, {nameof(WindIconId)}: {WindIconId}, {nameof(TimeKmFormatted)}: {TimeKmFormatted}";
    }
}
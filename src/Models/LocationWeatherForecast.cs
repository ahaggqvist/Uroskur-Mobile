namespace Uroskur.Models;

public class LocationWeatherForecast
{
    public int Km { get; set; }

    public DateTime DateTime { get; set; }

    public HourlyWeatherForecast HourlyWeatherForecast { get; set; } = new();

    public string WeatherIcon { get; set; } = string.Empty;

    public string WindIcon { get; set; } = string.Empty;

    public string WindIconId { get; set; } = string.Empty;

    public override string ToString()
    {
        return
            $"{nameof(Km)}: {Km}, {nameof(DateTime)}: {DateTime}, {nameof(HourlyWeatherForecast)}: {HourlyWeatherForecast}, {nameof(WeatherIcon)}: {WeatherIcon}, {nameof(WindIcon)}: {WindIcon}, {nameof(WindIconId)}: {WindIconId}";
    }
}
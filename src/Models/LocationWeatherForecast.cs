namespace Uroskur.Models;

public class LocationWeatherForecast
{
    public int Km { get; init; }

    public DateTime DateTime { get; init; }

    public HourlyWeatherForecast? HourlyWeatherForecast { get; init; }

    public string? WeatherIcon { get; init; }

    public string? WindIcon { get; init; }

    public string? WindIconId { get; init; }

    public override string ToString()
    {
        return
            $"{nameof(Km)}: {Km}, {nameof(DateTime)}: {DateTime}, {nameof(HourlyWeatherForecast)}: {HourlyWeatherForecast}, {nameof(WeatherIcon)}: {WeatherIcon}, {nameof(WindIcon)}: {WindIcon}, {nameof(WindIconId)}: {WindIconId}";
    }
}
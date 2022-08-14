namespace Uroskur.Models;

public class HourlyWeatherForecast
{
    public DateTime Dt { get; set; }
    public long UnixTimestamp { get; set; }
    public double Temp { get; set; }
    public double FeelsLike { get; set; }
    public double Uvi { get; set; }
    public double Cloudiness { get; set; }
    public double WindSpeed { get; set; }
    public double WindGust { get; set; }
    public double WindDeg { get; set; }
    public double Pop { get; set; }

    public double PrecipitationAmount { get; set; }
    public string Icon { get; set; } = string.Empty;

    public override string ToString()
    {
        return
            $"{nameof(Dt)}: {Dt}, {nameof(UnixTimestamp)}: {UnixTimestamp}, {nameof(Temp)}: {Temp}, {nameof(FeelsLike)}: {FeelsLike}, {nameof(Uvi)}: {Uvi}, {nameof(Cloudiness)}: {Cloudiness}, {nameof(WindSpeed)}: {WindSpeed}, {nameof(WindGust)}: {WindGust}, {nameof(WindDeg)}: {WindDeg}, {nameof(Pop)}: {Pop}, {nameof(Icon)}: {Icon}";
    }
}
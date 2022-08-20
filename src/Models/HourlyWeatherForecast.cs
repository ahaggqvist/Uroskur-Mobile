namespace Uroskur.Models;

public class HourlyWeatherForecast
{
    public DateTime Dt { get; init; }
    public long UnixTimestamp { get; init; }
    public double Temp { get; init; }
    public double FeelsLike { get; init; }
    public double Uvi { get; init; }
    public double Cloudiness { get; init; }
    public double WindSpeed { get; init; }
    public double WindGust { get; init; }
    public double WindDeg { get; init; }
    public double Pop { get; init; }
    public double PrecipitationAmount { get; init; }
    public string Icon { get; init; } = string.Empty;

    public override string ToString()
    {
        return
            $"{nameof(Dt)}: {Dt}, {nameof(UnixTimestamp)}: {UnixTimestamp}, {nameof(Temp)}: {Temp}, {nameof(FeelsLike)}: {FeelsLike}, {nameof(Uvi)}: {Uvi}, {nameof(Cloudiness)}: {Cloudiness}, {nameof(WindSpeed)}: {WindSpeed}, {nameof(WindGust)}: {WindGust}, {nameof(WindDeg)}: {WindDeg}, {nameof(Pop)}: {Pop}, {nameof(PrecipitationAmount)}: {PrecipitationAmount}, {nameof(Icon)}: {Icon}";
    }
}
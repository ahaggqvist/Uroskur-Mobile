namespace Uroskur.Models;

public class WeatherForecastParameters
{
    public Routes? Routes { get; set; }

    public int DayId { get; set; } = 0;

    public TimeSpan? Time { get; set; }

    public int SpeedId { get; set; } = 0;

    public int WeatherForecastProviderId { get; set; } = 0;

    public override string ToString()
    {
        return
            $"{nameof(Routes)}: {Routes}, {nameof(DayId)}: {DayId}, {nameof(Time)}: {Time}, {nameof(SpeedId)}: {SpeedId}, {nameof(WeatherForecastProviderId)}: {WeatherForecastProviderId}";
    }
}
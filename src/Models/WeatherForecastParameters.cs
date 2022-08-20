namespace Uroskur.Models;

public class WeatherForecastParameters
{
    public Routes? Routes { get; init; }

    public int DayId { get; init; }

    public TimeSpan? Time { get; init; }

    public int SpeedId { get; init; }

    public int WeatherForecastProviderId { get; init; }

    public override string ToString()
    {
        return
            $"{nameof(Routes)}: {Routes}, {nameof(DayId)}: {DayId}, {nameof(Time)}: {Time}, {nameof(SpeedId)}: {SpeedId}, {nameof(WeatherForecastProviderId)}: {WeatherForecastProviderId}";
    }
}
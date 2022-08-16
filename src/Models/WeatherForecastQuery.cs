﻿namespace Uroskur.Models;

public class WeatherForecastQuery
{
    public Routes? Routes { get; set; }

    public string? Day { get; set; }

    public TimeSpan? Time { get; set; }

    public int? Speed { get; set; }

    public int? Provider { get; set; }

    public override string ToString()
    {
        return $"{nameof(Routes)}: {Routes}, {nameof(Day)}: {Day}, {nameof(Time)}: {Time}, {nameof(Speed)}: {Speed}";
    }
}
namespace Uroskur.Utils;

public static class ChartHelper
{
    public static LineChart CreateTempChart(IEnumerable<LocationWeatherForecast> locationForecasts)
    {
        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 32,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            IsAnimated = true,
            BackgroundColor = SKColor.Parse("#fff"),
            LabelColor = SKColor.Parse("#000"),
            LabelTextSize = 25,
            ValueLabelTextSize = 25,
            SerieLabelTextSize = 25,
            LegendOption = SeriesLegendOption.Top,
            ShowYAxisLines = false,
            ShowYAxisText = false,
            EnableYFadeOutGradient = false,
            Series = new List<ChartSerie>
            {
                new()
                {
                    Name = "Temp °C",
                    Color = SKColor.Parse("#FC4C02"),
                    Entries = TempEntries(locationForecasts)
                }
            }
        };
    }

    public static LineChart CreateChanceOfRainChart(IEnumerable<LocationWeatherForecast> locationForecasts)
    {
        var forecasts = locationForecasts.ToImmutableArray();

        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 32,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            IsAnimated = true,
            BackgroundColor = SKColor.Parse("#fff"),
            LabelColor = SKColor.Parse("#000"),
            LabelTextSize = 25,
            ValueLabelTextSize = 25,
            SerieLabelTextSize = 25,
            LegendOption = SeriesLegendOption.Top,
            ShowYAxisLines = false,
            ShowYAxisText = false,
            EnableYFadeOutGradient = false,
            Series = new List<ChartSerie>
            {
                new()
                {
                    Name = "Chance of Rain %",
                    Color = SKColor.Parse("#FC4C02"),
                    Entries = ChanceOfRainEntries(forecasts)
                }
            }
        };
    }

    public static LineChart CreateUvChart(IEnumerable<LocationWeatherForecast> locationForecasts)
    {
        var forecasts = locationForecasts.ToImmutableArray();

        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 32,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            IsAnimated = true,
            BackgroundColor = SKColor.Parse("#fff"),
            LabelColor = SKColor.Parse("#000"),
            LabelTextSize = 25,
            ValueLabelTextSize = 25,
            SerieLabelTextSize = 25,
            LegendOption = SeriesLegendOption.Top,
            ShowYAxisLines = false,
            ShowYAxisText = false,
            EnableYFadeOutGradient = false,
            Series = new List<ChartSerie>
            {
                new()
                {
                    Name = "UV index",
                    Color = SKColor.Parse("#FC4C02"),
                    Entries = UvEntries(forecasts)
                },
                new()
                {
                    Name = "Cloudiness %",
                    Color = SKColor.Parse("#4dc9fe"),
                    Entries = CloudinessEntries(forecasts, false)
                }
            }
        };
    }

    public static LineChart CreateWindChart(IEnumerable<LocationWeatherForecast> locationForecasts)
    {
        var forecasts = locationForecasts.ToImmutableArray();

        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 32,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            IsAnimated = true,
            BackgroundColor = SKColor.Parse("#fff"),
            LabelColor = SKColor.Parse("#000"),
            LabelTextSize = 25,
            ValueLabelTextSize = 25,
            SerieLabelTextSize = 25,
            LegendOption = SeriesLegendOption.Top,
            ShowYAxisLines = false,
            ShowYAxisText = false,
            EnableYFadeOutGradient = false,
            Series = new List<ChartSerie>
            {
                new()
                {
                    Name = "Wind Speed m/s",
                    Color = SKColor.Parse("#FC4C02"),
                    Entries = WindSpeedEntries(forecasts)
                },
                new()
                {
                    Name = "Wind Gust m/s",
                    Color = SKColor.Parse("#4dc9fe"),
                    Entries = WindGustEntries(forecasts, false)
                }
            }
        };
    }

    private static IEnumerable<ChartEntry> TempEntries(IEnumerable<LocationWeatherForecast> locationForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyForecast in locationForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var temp = Math.Round(hourlyForecast.Temp, 1);
            var chartEntry = new ChartEntry((float?)temp)
            {
                ValueLabel = temp.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> ChanceOfRainEntries(IEnumerable<LocationWeatherForecast> locationForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyForecast in locationForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var chanceOfRain = Math.Round(hourlyForecast.Pop * 100);
            var chartEntry = new ChartEntry((float?)chanceOfRain)
            {
                ValueLabel = chanceOfRain.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> CloudinessEntries(IEnumerable<LocationWeatherForecast> locationForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyForecast in locationForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var cloudiness = hourlyForecast.Cloudiness;
            var chartEntry = new ChartEntry((float?)cloudiness)
            {
                ValueLabel = cloudiness.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> UvEntries(IEnumerable<LocationWeatherForecast> locationForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyForecast in locationForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var uvi = hourlyForecast.Uvi;
            var chartEntry = new ChartEntry((float?)uvi)
            {
                ValueLabel = uvi.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> WindSpeedEntries(IEnumerable<LocationWeatherForecast> locationForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyForecast in locationForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var windSpeed = hourlyForecast.WindSpeed;
            var chartEntry = new ChartEntry((float?)windSpeed)
            {
                ValueLabel = windSpeed.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> WindGustEntries(IEnumerable<LocationWeatherForecast> locationForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyForecast in locationForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var windGust = hourlyForecast.WindGust;
            var chartEntry = new ChartEntry((float?)windGust)
            {
                ValueLabel = windGust.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }
}
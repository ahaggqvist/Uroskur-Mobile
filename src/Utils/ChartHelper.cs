﻿namespace Uroskur.Utils;

public static class ChartHelper
{
#if WINDOWS
    private const int LabelTextSize = 15;
    private const int ValueLabelTextSize = 15;
    private const int SerieLabelTextSize = 15;
#else
    private const int LabelTextSize = 25;
    private const int ValueLabelTextSize = 25;
    private const int SerieLabelTextSize = 25;
#endif

    public static LineChart CreateTempChart(IEnumerable<LocationWeatherForecast> locationWeatherForecasts)
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
            LabelTextSize = LabelTextSize,
            ValueLabelTextSize = ValueLabelTextSize,
            SerieLabelTextSize = SerieLabelTextSize,
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
                    Entries = TempEntries(locationWeatherForecasts)
                }
            }
        };
    }

    public static LineChart CreateChanceOfRainChart(IEnumerable<LocationWeatherForecast> locationWeatherForecasts)
    {
        var weatherForecasts = locationWeatherForecasts.ToImmutableArray();

        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 32,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            IsAnimated = true,
            BackgroundColor = SKColor.Parse("#fff"),
            LabelColor = SKColor.Parse("#000"),
            LabelTextSize = LabelTextSize,
            ValueLabelTextSize = ValueLabelTextSize,
            SerieLabelTextSize = SerieLabelTextSize,
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
                    Entries = ChanceOfRainEntries(weatherForecasts)
                },
                new()
                {
                    Name = "Rain mm",
                    Color = SKColor.Parse("#4dc9fe"),
                    Entries = PrecipitationAmountEntries(weatherForecasts)
                }
            }
        };
    }

    public static LineChart CreateUvChart(IEnumerable<LocationWeatherForecast> locationWeatherForecasts)
    {
        var weatherForecasts = locationWeatherForecasts.ToImmutableArray();

        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 32,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            IsAnimated = true,
            BackgroundColor = SKColor.Parse("#fff"),
            LabelColor = SKColor.Parse("#000"),
            LabelTextSize = LabelTextSize,
            ValueLabelTextSize = ValueLabelTextSize,
            SerieLabelTextSize = SerieLabelTextSize,
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
                    Entries = UvEntries(weatherForecasts)
                },
                new()
                {
                    Name = "Cloudiness %",
                    Color = SKColor.Parse("#4dc9fe"),
                    Entries = CloudinessEntries(weatherForecasts, false)
                }
            }
        };
    }

    public static LineChart CreateWindChart(IEnumerable<LocationWeatherForecast> locationWeatherForecasts)
    {
        var weatherForecasts = locationWeatherForecasts.ToImmutableArray();

        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 32,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            IsAnimated = true,
            BackgroundColor = SKColor.Parse("#fff"),
            LabelColor = SKColor.Parse("#000"),
            LabelTextSize = LabelTextSize,
            ValueLabelTextSize = ValueLabelTextSize,
            SerieLabelTextSize = SerieLabelTextSize,
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
                    Entries = WindSpeedEntries(weatherForecasts)
                },
                new()
                {
                    Name = "Wind Gust m/s",
                    Color = SKColor.Parse("#4dc9fe"),
                    Entries = WindGustEntries(weatherForecasts, false)
                }
            }
        };
    }

    private static IEnumerable<ChartEntry> TempEntries(IEnumerable<LocationWeatherForecast> locationWeatherForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyWeatherForecast in locationWeatherForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var temp = Math.Round(hourlyWeatherForecast.Temp, 1);
            var chartEntry = new ChartEntry((float?)temp)
            {
                ValueLabel = temp.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyWeatherForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> ChanceOfRainEntries(IEnumerable<LocationWeatherForecast> locationWeatherForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyWeatherForecast in locationWeatherForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var chanceOfRain = Math.Round(hourlyWeatherForecast.Pop * 100);
            var chartEntry = new ChartEntry((float?)chanceOfRain)
            {
                ValueLabel = chanceOfRain.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyWeatherForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> CloudinessEntries(IEnumerable<LocationWeatherForecast> locationWeatherForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyWeatherForecast in locationWeatherForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var cloudiness = hourlyWeatherForecast.Cloudiness;
            var chartEntry = new ChartEntry((float?)cloudiness)
            {
                ValueLabel = cloudiness.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyWeatherForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> UvEntries(IEnumerable<LocationWeatherForecast> locationWeatherForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyWeatherForecast in locationWeatherForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var uvi = hourlyWeatherForecast.Uvi;
            var chartEntry = new ChartEntry((float?)uvi)
            {
                ValueLabel = uvi.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyWeatherForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> WindSpeedEntries(IEnumerable<LocationWeatherForecast> locationWeatherForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyWeatherForecast in locationWeatherForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var windSpeed = hourlyWeatherForecast.WindSpeed;
            var chartEntry = new ChartEntry((float?)windSpeed)
            {
                ValueLabel = windSpeed.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyWeatherForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> WindGustEntries(IEnumerable<LocationWeatherForecast> locationWeatherForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyWeatherForecast in locationWeatherForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var windGust = hourlyWeatherForecast.WindGust;
            var chartEntry = new ChartEntry((float?)windGust)
            {
                ValueLabel = windGust.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyWeatherForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static IEnumerable<ChartEntry> PrecipitationAmountEntries(IEnumerable<LocationWeatherForecast> locationWeatherForecasts, bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var hourlyWeatherForecast in locationWeatherForecasts.Select(l => l.HourlyWeatherForecast))
        {
            var precipitationAmount = hourlyWeatherForecast.PrecipitationAmount;
            var chartEntry = new ChartEntry((float?)precipitationAmount)
            {
                ValueLabel = precipitationAmount.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? hourlyWeatherForecast.Dt.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }
}
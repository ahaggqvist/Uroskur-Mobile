namespace Uroskur.Utils;

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
        var forecasts = locationWeatherForecasts.ToImmutableArray();
        return CreateLineChart(new List<ChartSerie>
        {
            new()
            {
                Name = "Temp (°C)",
                Color = SKColor.Parse("#fc4c02"),
                Entries = GetChartEntries(forecasts, "Temp")
            }
        });
    }

    public static LineChart CreateChanceOfRainChart(IEnumerable<LocationWeatherForecast> locationWeatherForecasts)
    {
        var forecasts = locationWeatherForecasts.ToImmutableArray();
        return CreateLineChart(new List<ChartSerie>
        {
            new()
            {
                Name = "Chance of Rain (%)",
                Color = SKColor.Parse("#fc4c02"),
                Entries = GetChartEntries(forecasts, "Pop")
            },
            new()
            {
                Name = "Rain (mm)",
                Color = SKColor.Parse("#4dc9fe"),
                Entries = GetChartEntries(forecasts, "PrecipitationAmount", false)
            }
        });
    }

    public static LineChart CreateUvChart(IEnumerable<LocationWeatherForecast> locationWeatherForecasts)
    {
        var forecasts = locationWeatherForecasts.ToImmutableArray();
        return CreateLineChart(new List<ChartSerie>
        {
            new()
            {
                Name = "UV index",
                Color = SKColor.Parse("#fc4c02"),
                Entries = GetChartEntries(forecasts, "Uvi")
            },
            new()
            {
                Name = "Cloudiness (%)",
                Color = SKColor.Parse("#4dc9fe"),
                Entries = GetChartEntries(forecasts, "Cloudiness", false)
            }
        });
    }

    public static LineChart CreateWindChart(IEnumerable<LocationWeatherForecast> locationWeatherForecasts)
    {
        var forecasts = locationWeatherForecasts.ToImmutableArray();
        return CreateLineChart(new List<ChartSerie>
        {
            new()
            {
                Name = "Wind Speed (m/s)",
                Color = SKColor.Parse("#fc4c02"),
                Entries = GetChartEntries(forecasts, "WindSpeed")
            },
            new()
            {
                Name = "Wind Gust (m/s)",
                Color = SKColor.Parse("#4dc9fe"),
                Entries = GetChartEntries(forecasts, "WindGust", false)
            }
        });
    }

    private static LineChart CreateLineChart(IEnumerable<ChartSerie> series)
    {
        return new LineChart
        {
            LineMode = LineMode.Spline,
            LineAreaAlpha = 16,
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
            EnableYFadeOutGradient = true,
            Series = series
        };
    }

    private static IEnumerable<ChartEntry> GetChartEntries(
        IEnumerable<LocationWeatherForecast> locationWeatherForecasts, string propertyName, bool withLabel = true)
    {
        var chartEntries = (from locationWeatherForecast in locationWeatherForecasts
            where locationWeatherForecast.HourlyWeatherForecast != null
            let hourlyWeatherForecast = locationWeatherForecast.HourlyWeatherForecast
            let value = GetValue(hourlyWeatherForecast, propertyName)
            select CreateChartEntry(value, locationWeatherForecast.DateTime, withLabel)).ToList();

        static float GetValue(HourlyWeatherForecast hourlyWeatherForecast, string propertyName)
        {
            var type = typeof(HourlyWeatherForecast);
            var field = type.GetProperty(propertyName);
            var obj = field?.GetValue(hourlyWeatherForecast);
            return Convert.ToSingle(obj);
        }

        static ChartEntry CreateChartEntry(float value, DateTime dateTime, bool withLabel)
        {
            return new ChartEntry(value)
            {
                ValueLabel = value.ToString(CultureInfo.CurrentCulture),
                Label = withLabel ? dateTime.ToString("H:mm") : null
            };
        }

        return chartEntries;
    }
}
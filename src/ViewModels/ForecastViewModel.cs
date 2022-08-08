namespace Uroskur.ViewModels;

[QueryProperty(nameof(ForecastQuery), nameof(ForecastQuery))]
public partial class ForecastViewModel : BaseViewModel
{
    private readonly AppSettings _appSettings;
    private readonly IWeatherService _weatherService;
    [ObservableProperty] private LineChart? _chanceOfRainLineChart;
    [ObservableProperty] private string? _emptyViewMessage;
    [ObservableProperty] private string? _forecastIssuedAt;
    [ObservableProperty] private string? _forecastIssuedFor;
    [ObservableProperty] private ForecastQuery? _forecastQuery;
    [ObservableProperty] private LineChart? _tempLineChart;


    public ForecastViewModel(IWeatherService weatherService, AppSettings appSettings)
    {
        _weatherService = weatherService;
        _appSettings = appSettings;
    }

    public ObservableCollection<LocationForecast> LocationForecasts { get; } = new();

    partial void OnForecastQueryChanged(ForecastQuery? value)
    {
        Title = value?.Routes?.Name;
    }

    public async Task GetForecastAsync()
    {
        try
        {
            var today = DateTime.Today;
            if (_forecastQuery is { Day: "Tomorrow" })
            {
                today = today.AddDays(1);
            }

            var timeSpan = _forecastQuery!.Time;
            var hour = timeSpan!.Value.Hours;
            var issuedFor = today.AddHours(hour).AddMinutes(0).AddSeconds(0).ToLocalTime();
            if (_appSettings.IsDevelopment)
            {
                issuedFor = new DateTime(2022, 2, 20, 18, 0, 0).ToLocalTime();
            }

            var unixTime = (int)issuedFor.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var route = _forecastQuery?.Routes;
            var athlete = route?.Athlete;
            var athleteId = athlete?.Id.ToString();
            var routeId = route?.Id.ToString();
            var openWeatherForecasts = await _weatherService.FindForecastAsync(routeId, athleteId);

            var openWeatherForecastsArray = openWeatherForecasts.ToImmutableArray();
            if (openWeatherForecastsArray.Length > 0)
            {
                var openWeatherForecast = openWeatherForecastsArray.ElementAt(0);
                var dt = openWeatherForecast.Hourly![0].Dt;
                if (dt != null)
                {
                    var issuedAt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds((double)dt);
                    _forecastIssuedAt = $"OpenWeather Forecast Issued at {issuedAt:ddd, d MMM H:mm}";
                    OnPropertyChanged(nameof(ForecastIssuedAt));
                }

                _forecastIssuedFor = $"{issuedFor:dddd, d MMM}";
                OnPropertyChanged(nameof(ForecastIssuedFor));
            }

            foreach (var (openWeatherForecast, index) in openWeatherForecastsArray.WithIndex())
            {
                var km = index * 10 + 10;
                var speed = _forecastQuery!.Speed!.Value;
                var time = km / speed;
                var seconds = 3600 * time + unixTime;
                var hourly = openWeatherForecast.Hourly?.Find(h => h.Dt!.Value == seconds);

                if (hourly == null)
                {
                    continue;
                }

                var weatherIconId = hourly?.Weather?[0].Id;
                var windDeg = hourly?.WindDeg!.Value ?? 0L;
                var windIconId = WindDirection[(int)Math.Round(windDeg / 22.5, 0)];
                var unixTimeDateTime = UnixTimeToDateTime(km, speed, unixTime);

                var forecast = new LocationForecast
                {
                    Km = km,
                    Hourly = hourly,
                    UnixDateTime = unixTimeDateTime,
                    WeatherIcon = WeatherIconsDictionary[weatherIconId!.Value],
                    WindIcon = WindIconsDictionary[windIconId],
                    WindIconId = windIconId
                };

                LocationForecasts.Add(forecast);
            }

            if (LocationForecasts.Count != 0)
            {
                GetTempChart();

                GetChanceOfRainChart();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get routes: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    public void GetTempChart()
    {
        TempLineChart = new LineChart
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
                    Entries = TempEntries()
                },
                new()
                {
                    Name = "Feels Like °C",
                    Color = SKColor.Parse("#4dc9fe"),
                    Entries = FeelsLikeEntries(false)
                }
            }
        };
    }

    public void GetChanceOfRainChart()
    {
        ChanceOfRainLineChart = new LineChart
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
                    Entries = ChanceOfRainEntries()
                },
                new()
                {
                    Name = "Cloudiness %",
                    Color = SKColor.Parse("#4dc9fe"),
                    Entries = CloudinessEntries(false)
                }
            }
        };
    }

    private IEnumerable<ChartEntry> TempEntries(bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var forecast in LocationForecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null)
            {
                continue;
            }

            var temp = Math.Round(hourly.Temp!.Value, 1);
            var chartEntry = new ChartEntry((float?)temp)
            {
                ValueLabel = temp.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? forecast.UnixDateTime.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private IEnumerable<ChartEntry> FeelsLikeEntries(bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var forecast in LocationForecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null)
            {
                continue;
            }

            var feelsLike = Math.Round(hourly.FeelsLike!.Value, 1);
            var chartEntry = new ChartEntry((float?)feelsLike)
            {
                ValueLabel = feelsLike.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? forecast.UnixDateTime.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private IEnumerable<ChartEntry> ChanceOfRainEntries(bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var forecast in LocationForecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null)
            {
                continue;
            }

            var chanceOfRain = Math.Round(hourly.Pop!.Value * 100);
            var chartEntry = new ChartEntry((float?)chanceOfRain)
            {
                ValueLabel = chanceOfRain.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? forecast.UnixDateTime.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private IEnumerable<ChartEntry> CloudinessEntries(bool withLabel = true)
    {
        var chartEntries = new List<ChartEntry>();

        foreach (var forecast in LocationForecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null)
            {
                continue;
            }

            var cloudiness = hourly.Clouds!.Value;
            var chartEntry = new ChartEntry(cloudiness)
            {
                ValueLabel = cloudiness.ToString(CultureInfo.InvariantCulture),
                Label = withLabel ? forecast.UnixDateTime.ToString("HH:mm") : null
            };

            chartEntries.Add(chartEntry);
        }

        return chartEntries;
    }

    private static DateTime UnixTimeToDateTime(int km, int speed, int unixTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
            .AddSeconds(3600 * ((double)km / speed) + unixTime).ToLocalTime();
    }
}
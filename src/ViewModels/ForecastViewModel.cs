namespace Uroskur.ViewModels;

[QueryProperty(nameof(ForecastRoute), nameof(ForecastRoute))]
public partial class ForecastViewModel : BaseViewModel
{
    private readonly AppSettings _appSettings;
    private readonly IOpenWeatherService _openWeatherService;

    private readonly Dictionary<long, string> _weatherIconsDictionary;

    private readonly string[] _windDirection =
        { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };

    private readonly Dictionary<string, string> _windIconsDictionary;

    [ObservableProperty] private LineChart? _chanceOfRainLineChart;

    [ObservableProperty] private string? _emptyViewMessage;

    [ObservableProperty] private string? _forecastIssuedAt;

    [ObservableProperty] private string? _forecastIssuedFor;

    [ObservableProperty] private ForecastRoute? _forecastRoute;

    [ObservableProperty] private LineChart? _tempLineChart;

    public ForecastViewModel(IOpenWeatherService openWeatherService, AppSettings appSettings)
    {
        _openWeatherService = openWeatherService;
        _appSettings = appSettings;

        _weatherIconsDictionary = new Dictionary<long, string>
        {
            { 200, "\uf01e" },
            { 201, "\uf01e" },
            { 202, "\uf01e" },
            { 210, "\uf016" },
            { 211, "\uf016" },
            { 212, "\uf016" },
            { 221, "\uf016" },
            { 230, "\uf01e" },
            { 231, "\uf01e" },
            { 232, "\uf01e" },
            { 300, "\uf01c" },
            { 301, "\uf01c" },
            { 302, "\uf019" },
            { 310, "\uf017" },
            { 311, "\uf019" },
            { 312, "\uf019" },
            { 313, "\uf01a" },
            { 314, "\uf019" },
            { 321, "\uf01c" },
            { 500, "\uf01c" },
            { 501, "\uf019" },
            { 502, "\uf019" },
            { 503, "\uf019" },
            { 504, "\uf019" },
            { 511, "\uf017" },
            { 520, "\uf01a" },
            { 521, "\uf01a" },
            { 522, "\uf01a" },
            { 531, "\uf01d" },
            { 600, "\uf01b" },
            { 601, "\uf01b" },
            { 602, "\uf0b5" },
            { 611, "\uf017" },
            { 612, "\uf017" },
            { 615, "\uf017" },
            { 616, "\uf017" },
            { 620, "\uf017" },
            { 621, "\uf01b" },
            { 622, "\uf01b" },
            { 701, "\uf01a" },
            { 711, "\uf062" },
            { 721, "\uf0b6" },
            { 731, "\uf063" },
            { 741, "\uf014" },
            { 761, "\uf063" },
            { 762, "\uf063" },
            { 771, "\uf011" },
            { 781, "\uf056" },
            { 800, "\uf00d" },
            { 801, "\uf011" },
            { 802, "\uf011" },
            { 803, "\uf012" },
            { 804, "\uf013" },
            { 901, "\uf01d" },
            { 902, "\uf073" },
            { 903, "\uf076" },
            { 904, "\uf072" },
            { 905, "\uf021" },
            { 906, "\uf015" },
            { 957, "\uf050" }
        };

        _windIconsDictionary = new Dictionary<string, string>
        {
            { "N", "\uf058" },
            { "NNE", "\uf057" },
            { "NE", "\uf057" },
            { "ENE", "\uf057" },
            { "E", "\uf04d" },
            { "ESE", "\uf088" },
            { "SE", "\uf088" },
            { "SSE", "\uf088" },
            { "S", "\uf044" },
            { "SSW", "\uf043" },
            { "SW", "\uf043" },
            { "WSW", "\uf043" },
            { "W", "\uf048" },
            { "WNW", "\uf087" },
            { "NW", "\uf087" },
            { "NNW", "\uf087" }
        };
    }

    public ObservableCollection<Forecast> Forecasts { get; } = new();

    public async Task GetForecastAsync()
    {
        try
        {
            var today = DateTime.Today;
            if (_forecastRoute is { Day: "Tomorrow" }) today = today.AddDays(1);

            var timeSpan = _forecastRoute!.Time;
            var hour = timeSpan!.Value.Hours;
            var issuedFor = today.AddHours(hour).AddMinutes(0).AddSeconds(0).ToLocalTime();
            if (_appSettings.IsDevelopment) issuedFor = new DateTime(2022, 2, 20, 18, 0, 0).ToLocalTime();

            var unixTime = (int)issuedFor.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var route = _forecastRoute?.Routes;
            var athlete = route?.Athlete;
            var athleteId = athlete?.Id.ToString();
            var routeId = route?.Id.ToString();
            var temperatures = await _openWeatherService.FindForecastAsync(routeId, athleteId);

            var temperaturesList = temperatures.ToList();
            if (temperaturesList.Count > 0)
            {
                var temperature = temperaturesList.ElementAt(0);
                var dt = temperature.Hourly![0].Dt;
                if (dt != null)
                {
                    var issuedAt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds((double)dt);
                    _forecastIssuedAt = $"OpenWeather Forecast Issued at {issuedAt:ddd, d MMM H:mm}";
                    OnPropertyChanged(nameof(ForecastIssuedAt));
                }

                _forecastIssuedFor = $"{issuedFor:dddd, d MMM}";
                OnPropertyChanged(nameof(ForecastIssuedFor));
            }

            foreach (var (temperature, index) in temperaturesList.WithIndex())
            {
                var km = index * 10 + 10;
                var speed = _forecastRoute!.Speed!.Value;
                var time = km / speed;
                var seconds = 3600 * time + unixTime;
                var hourly = temperature.Hourly?.Find(h => h.Dt!.Value == seconds);

                if (hourly == null) continue;

                var weatherIconId = hourly?.Weather?[0].Id;
                var windDeg = hourly?.WindDeg!.Value ?? 0L;
                var windIconId = _windDirection[(int)Math.Round(windDeg / 22.5, 0)];
                var unixTimeDateTime = UnixTimeToDateTime(km, speed, unixTime);

                var forecast = new Forecast
                {
                    Km = km,
                    Hourly = hourly,
                    UnixDateTime = unixTimeDateTime,
                    WeatherIcon = _weatherIconsDictionary[weatherIconId!.Value],
                    WindIcon = _windIconsDictionary[windIconId],
                    WindIconId = windIconId
                };

                Forecasts.Add(forecast);
            }

            if (Forecasts.Count != 0)
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

        foreach (var forecast in Forecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null) continue;

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

        foreach (var forecast in Forecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null) continue;

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

        foreach (var forecast in Forecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null) continue;

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

        foreach (var forecast in Forecasts)
        {
            var hourly = forecast.Hourly;
            if (hourly == null) continue;

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
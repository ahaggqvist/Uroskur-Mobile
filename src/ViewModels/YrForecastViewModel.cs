namespace Uroskur.ViewModels;

[QueryProperty(nameof(ForecastQuery), nameof(ForecastQuery))]
public partial class YrForecastViewModel : BaseViewModel
{
    private readonly IForecastService _forecastService;
    [ObservableProperty] private LineChart? _chanceOfRainLineChart;
    [ObservableProperty] private string? _emptyViewMessage;
    [ObservableProperty] private string? _forecastIssuedAt;
    [ObservableProperty] private string? _forecastIssuedFor;
    [ObservableProperty] private ForecastQuery? _forecastQuery;
    [ObservableProperty] private LineChart? _tempLineChart;
    [ObservableProperty] private LineChart? _uvLineChart;
    [ObservableProperty] private LineChart? _windLineChart;

    public YrForecastViewModel(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    public ObservableCollection<LocationForecast> LocationForecasts { get; } = new();

    public async Task WeatherForecastAsync()
    {
        Title = _forecastQuery?.Routes?.Name;

        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;

            var today = DateTime.Today;
            if (_forecastQuery is { Day: "Tomorrow" })
            {
                today = today.AddDays(1);
            }

            var timeSpan = _forecastQuery!.Time;
            var hour = timeSpan!.Value.Hours;
            var issuedFor = today.AddHours(hour).AddMinutes(0).AddSeconds(0).ToLocalTime();
            var issuedForUnixTimestamp = DateTimeHelper.DateTimeToUnixTimestamp(issuedFor);
            var route = _forecastQuery?.Routes;
            var athlete = route?.Athlete;
            var athleteId = athlete?.Id.ToString();
            var routeId = route?.Id.ToString();
            var forecasts = await _forecastService.FindYrForecastsAsync(routeId, athleteId);

            var forecastsArray = forecasts.ToImmutableArray();
            if (forecastsArray.Length > 0)
            {
                var hourlyForecast = forecastsArray[0].HourlyForecasts.ElementAt(0);
                var issuedAt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(hourlyForecast.UnixTimestamp);

                _forecastIssuedAt = $"Yr Forecast Issued at {issuedAt:ddd, d MMM H:mm}";
                OnPropertyChanged(nameof(ForecastIssuedAt));


                _forecastIssuedFor = $"{issuedFor:dddd, d MMM}";
                OnPropertyChanged(nameof(ForecastIssuedFor));
            }

            foreach (var (forecast, index) in forecastsArray.WithIndex())
            {
                var km = index * 10 + 10;
                var speed = _forecastQuery!.Speed!.Value;
                var time = km / speed;
                var seconds = 3600 * time + issuedForUnixTimestamp;
                var hourlyForecast = forecast.HourlyForecasts.ToImmutableList().Find(h => Math.Abs(h.UnixTimestamp - seconds) < 0.000000001);

                if (hourlyForecast == null)
                {
                    Debug.WriteLine("Hourly forecast is null");
                    continue;
                }

                var windDeg = hourlyForecast?.WindDeg ?? 0L;
                var windIconId = WindDirection[(int)Math.Round(windDeg / 22.5, 0)];
                var locationDt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
                    .AddSeconds(3600 * ((double)km / speed) + issuedForUnixTimestamp).ToLocalTime();

                var locationForecast = new LocationForecast
                {
                    Km = km,
                    HourlyForecast = hourlyForecast!,
                    Dt = locationDt,
                    WeatherIcon = hourlyForecast!.Icon,
                    WindIcon = WindIconsDictionary[windIconId],
                    WindIconId = windIconId
                };

                LocationForecasts.Add(locationForecast);
            }

            if (LocationForecasts.Count != 0)
            {
                TempLineChart = ChartHelper.CreateTempChart(LocationForecasts);
                ChanceOfRainLineChart = ChartHelper.CreateChanceOfRainChart(LocationForecasts);
                UvLineChart = ChartHelper.CreateUvChart(LocationForecasts);
                WindLineChart = ChartHelper.CreateWindChart(LocationForecasts);
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
}
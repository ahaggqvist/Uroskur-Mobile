namespace Uroskur.ViewModels;

[QueryProperty(nameof(WeatherForecastQuery), nameof(WeatherForecastQuery))]
public partial class YrWeatherForecastViewModel : BaseViewModel
{
    private readonly IWeatherForecastService _weatherForecastService;
    [ObservableProperty] private LineChart? _chanceOfRainLineChart;
    [ObservableProperty] private string? _emptyViewMessage;
    [ObservableProperty] private string? _forecastIssuedAt;
    [ObservableProperty] private string? _forecastIssuedFor;
    [ObservableProperty] private LineChart? _tempLineChart;
    [ObservableProperty] private LineChart? _uvLineChart;
    [ObservableProperty] private WeatherForecastQuery? _weatherForecastQuery;
    [ObservableProperty] private LineChart? _windLineChart;

    public YrWeatherForecastViewModel(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    public ObservableCollection<LocationWeatherForecast> LocationWeatherForecasts { get; } = new();

    public async Task WeatherForecastAsync()
    {
        Title = _weatherForecastQuery?.Routes?.Name;

        await Task.Delay(500);

        if (IsBusy)
        {
            return;
        }

        if (LocationWeatherForecasts.Count != 0)
        {
            LocationWeatherForecasts.Clear();
        }

        try
        {
            IsBusy = true;

            var today = DateTime.Today;
            if (_weatherForecastQuery is { Day: "Tomorrow" })
            {
                today = today.AddDays(1);
            }

            var timeSpan = _weatherForecastQuery!.Time;
            var hour = timeSpan!.Value.Hours;
            var issuedFor = today.AddHours(hour).AddMinutes(0).AddSeconds(0).ToLocalTime();
            var issuedForUnixTimestamp = DateTimeHelper.DateTimeToUnixTimestamp(issuedFor);
            var route = _weatherForecastQuery?.Routes;
            var athlete = route?.Athlete;
            var athleteId = athlete?.Id.ToString();
            var routeId = route?.Id.ToString();
            var weatherForecasts = await _weatherForecastService.FindYrWeatherForecastsAsync(routeId, athleteId);

            var weatherForecastsArray = weatherForecasts.ToImmutableArray();
            if (weatherForecastsArray.Length > 0)
            {
                var hourlyForecast = weatherForecastsArray[0].HourlyWeatherForecasts.ElementAt(0);
                var issuedAt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(hourlyForecast.UnixTimestamp);

                ForecastIssuedAt = $"Yr Weather forecast issued at {issuedAt:ddd, d MMM H:mm}";
                ForecastIssuedFor = $"{issuedFor:dddd, d MMM}";
            }

            foreach (var (weatherForecast, index) in weatherForecastsArray.WithIndex())
            {
                var km = index * 10 + 10;
                var speed = _weatherForecastQuery!.Speed!.Value;
                var time = km / speed;
                var seconds = 3600 * time + issuedForUnixTimestamp;
                var hourlyWeatherForecast = weatherForecast.HourlyWeatherForecasts.ToImmutableList().Find(h => Math.Abs(h.UnixTimestamp - seconds) < 0.000000001);

                if (hourlyWeatherForecast == null)
                {
                    Debug.WriteLine("Hourly weather forecast is null");
                    continue;
                }

                var windDeg = hourlyWeatherForecast?.WindDeg ?? 0L;
                var windIconId = WindDirection[(int)Math.Round(windDeg / 22.5, 0)];
                var locationDt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
                    .AddSeconds(3600 * ((double)km / speed) + issuedForUnixTimestamp).ToLocalTime();

                var locationWeatherForecast = new LocationWeatherForecast
                {
                    Km = km,
                    HourlyWeatherForecast = hourlyWeatherForecast!,
                    Dt = locationDt,
                    WeatherIcon = hourlyWeatherForecast!.Icon,
                    WindIcon = WindIconsDictionary[windIconId],
                    WindIconId = windIconId
                };

                LocationWeatherForecasts.Add(locationWeatherForecast);
            }

            if (LocationWeatherForecasts.Count != 0)
            {
                TempLineChart = ChartHelper.CreateTempChart(LocationWeatherForecasts);
                ChanceOfRainLineChart = ChartHelper.CreateChanceOfRainChart(LocationWeatherForecasts);
                UvLineChart = ChartHelper.CreateUvChart(LocationWeatherForecasts);
                WindLineChart = ChartHelper.CreateWindChart(LocationWeatherForecasts);
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
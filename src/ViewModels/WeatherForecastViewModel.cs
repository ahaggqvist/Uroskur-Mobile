namespace Uroskur.ViewModels;

[QueryProperty(nameof(WeatherForecastParameters), nameof(WeatherForecastParameters))]
public partial class WeatherForecastViewModel : BaseViewModel
{
    private readonly IWeatherForecastService _weatherForecastService;
    [ObservableProperty] private LineChart? _chanceOfRainLineChart;
    [ObservableProperty] private string? _emptyViewMessage;
    [ObservableProperty] private string? _forecastIssuedAt;
    [ObservableProperty] private string? _forecastIssuedFor;
    [ObservableProperty] private DateTime? _sunrise;
    [ObservableProperty] private DateTime? _sunset;
    [ObservableProperty] private LineChart? _tempLineChart;
    [ObservableProperty] private LineChart? _uviLineChart;
    [ObservableProperty] private WeatherForecastParameters? _weatherForecastParameters;
    [ObservableProperty] private LineChart? _windLineChart;

    public WeatherForecastViewModel(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    public ObservableCollection<LocationWeatherForecast> LocationWeatherForecasts { get; } = new();

    public async Task WeatherForecastAsync()
    {
        Title = WeatherForecastParameters?.Routes?.Name!;

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
            var today = DateTime.Today;
            if (WeatherForecastParameters?.DayId == Day.Tomorrow.Id)
            {
                today = today.AddDays(1);
            }

            var timeSpan = WeatherForecastParameters!.Time;
            var hour = timeSpan!.Value.Hours;
            var issuedFor = today.AddHours(hour).AddMinutes(0).AddSeconds(0).ToLocalTime();
            var weatherForecastProvider = Enumeration.FromId<WeatherForecastProvider>(WeatherForecastParameters?.WeatherForecastProviderId ?? 0);
            var issuedForUnixTimestamp = DateTimeToUnixTimestamp(issuedFor);
            var route = WeatherForecastParameters?.Routes;
            var athlete = route?.Athlete;
            var athleteId = athlete?.Id.ToString();
            var routeId = route?.Id.ToString();
            var weatherForecasts = await _weatherForecastService.FindWeatherForecastsAsync(weatherForecastProvider, routeId, athleteId);

            var weatherForecastsArray = weatherForecasts.ToImmutableArray();
            if (weatherForecastsArray.Length > 0)
            {
                var weatherForecast = weatherForecastsArray[0];
                var hourlyForecast = weatherForecast.HourlyWeatherForecasts.ElementAt(0);
                var issuedAt =
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(hourlyForecast.UnixTimestamp);

                ForecastIssuedAt = $"{weatherForecastProvider} Weather Forecast updated at {issuedAt:ddd, d MMM H:mm}";
                ForecastIssuedFor = $"{issuedFor:dddd, d MMM}";


                if (WeatherForecastParameters?.DayId == Day.Today.Id)
                {
                    Sunrise = weatherForecast.SunriseToday;
                    Sunset = weatherForecast.SunsetToday;
                }
                else
                {
                    Sunrise = weatherForecast.SunriseTomorrow;
                    Sunset = weatherForecast.SunsetTomorrow;
                }
            }

            foreach (var (weatherForecast, index) in weatherForecastsArray.WithIndex())
            {
                var km = index * 10 + 10;
                var speed = int.Parse(Enumeration.FromId<Speed>(WeatherForecastParameters!.SpeedId).Name);
                var time = km / speed;
                var seconds = 3600 * time + issuedForUnixTimestamp;
                var hourlyWeatherForecast = weatherForecast.HourlyWeatherForecasts.ToImmutableList()
                    .Find(h => Math.Abs(h.UnixTimestamp - seconds) < 0.000000001);

                if (hourlyWeatherForecast == null)
                {
                    Debug.WriteLine("Hourly weather forecast is null.");
                    continue;
                }

                var windDeg = hourlyWeatherForecast.WindDeg;
                var windIconId = WindDirection[(int)Math.Round(windDeg / 22.5, 0)];
                var locationDt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
                    .AddSeconds(3600 * ((double)km / speed) + issuedForUnixTimestamp).ToLocalTime();

                var locationForecast = new LocationWeatherForecast
                {
                    Km = km,
                    HourlyWeatherForecast = hourlyWeatherForecast,
                    DateTime = locationDt,
                    WeatherIcon = hourlyWeatherForecast.Icon,
                    WindIcon = WindIconsDictionary[windIconId],
                    WindIconId = windIconId
                };

                LocationWeatherForecasts.Add(locationForecast);
            }

            if (LocationWeatherForecasts.Count != 0)
            {
                TempLineChart = ChartHelper.CreateTempChart(LocationWeatherForecasts);
                ChanceOfRainLineChart = ChartHelper.CreateChanceOfRainChart(LocationWeatherForecasts);
                UviLineChart = ChartHelper.CreateUviChart(LocationWeatherForecasts);
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
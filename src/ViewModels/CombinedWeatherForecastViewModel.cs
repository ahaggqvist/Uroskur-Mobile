using Extensions = Uroskur.Utils.Extensions;

namespace Uroskur.ViewModels;

[QueryProperty(nameof(WeatherForecastParameters), nameof(WeatherForecastParameters))]
public partial class CombinedWeatherForecastViewModel : BaseViewModel
{
    private readonly IWeatherForecastService _weatherForecastService;
    [ObservableProperty] private string? _emptyViewMessage;
    [ObservableProperty] private string? _forecastIssuedAt;
    [ObservableProperty] private string? _forecastIssuedFor;
    [ObservableProperty] private WeatherForecastParameters? _weatherForecastParameters;

    public CombinedWeatherForecastViewModel(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    public ObservableCollection<LocationWeatherForecast> OpenWeatherLocationWeatherForecasts { get; } = new();
    public ObservableCollection<LocationWeatherForecast> YrLocationWeatherForecasts { get; } = new();
    public ObservableCollection<LocationWeatherForecast> SmhiLocationWeatherForecasts { get; } = new();

    public async Task WeatherForecastAsync()
    {
        Title = _weatherForecastParameters?.Routes?.Name!;

        await Task.Delay(500);

        if (IsBusy)
        {
            return;
        }

        if (OpenWeatherLocationWeatherForecasts.Count != 0)
        {
            OpenWeatherLocationWeatherForecasts.Clear();
            YrLocationWeatherForecasts.Clear();
            SmhiLocationWeatherForecasts.Clear();
        }

        try
        {
            var today = DateTime.Today;
            if (_weatherForecastParameters?.DayId == Day.Tomorrow.Id)
            {
                today = today.AddDays(1);
            }

            var timeSpan = _weatherForecastParameters!.Time;
            var hour = timeSpan!.Value.Hours;
            var issuedFor = today.AddHours(hour).AddMinutes(0).AddSeconds(0).ToLocalTime();
            var issuedForUnixTimestamp = DateTimeToUnixTimestamp(issuedFor);
            var route = _weatherForecastParameters?.Routes;
            var athlete = route?.Athlete;
            var athleteId = athlete?.Id.ToString();
            var routeId = route?.Id.ToString();

            // Await all tasks
            var openWeatherFindTask = _weatherForecastService.FindWeatherForecastsAsync(OpenWeather, routeId, athleteId);
            var yrFindTask = _weatherForecastService.FindWeatherForecastsAsync(Yr, routeId, athleteId);
            var smhiFindTask = _weatherForecastService.FindWeatherForecastsAsync(Smhi, routeId, athleteId);
            var (openWeatherForecasts, yrWeatherForecasts, smhiWeatherForecasts) = await Extensions.WhenAll(openWeatherFindTask, yrFindTask, smhiFindTask);

            var weatherForecastsArray = openWeatherForecasts.ToImmutableArray();
            if (weatherForecastsArray.Length > 0)
            {
                var hourlyWeatherForecasts = weatherForecastsArray.FirstOrDefault();
                var hourlyForecast = hourlyWeatherForecasts?.HourlyWeatherForecasts.ElementAt(0);
                if (hourlyForecast != null)
                {
                    var issuedAt =
                        new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(hourlyForecast.UnixTimestamp);
                    ForecastIssuedAt = $"Weather Forecast updated at {issuedAt:ddd, d MMM H:mm}";
                }

                ForecastIssuedFor = $"{issuedFor:dddd, d MMM}";
            }

            AddLocationWeatherForecasts(OpenWeather, weatherForecastsArray, issuedForUnixTimestamp);
            AddLocationWeatherForecasts(Yr, yrWeatherForecasts, issuedForUnixTimestamp);
            AddLocationWeatherForecasts(Smhi, smhiWeatherForecasts, issuedForUnixTimestamp);
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

    private void AddLocationWeatherForecasts(WeatherForecastProvider weatherForecastProvider, IEnumerable<WeatherForecast> weatherForecastsArray, double issuedForUnixTimestamp)
    {
        foreach (var (weatherForecast, index) in weatherForecastsArray.WithIndex())
        {
            var km = index * 10 + 10;
            var speed = int.Parse(Enumeration.FromId<Speed>(_weatherForecastParameters!.SpeedId).Name);
            var time = km / speed;
            var seconds = 3600 * time + issuedForUnixTimestamp;
            var hourlyWeatherForecast = weatherForecast.HourlyWeatherForecasts.ToImmutableList()
                .Find(h => Math.Abs(h.UnixTimestamp - seconds) < 0.000000001);

            if (hourlyWeatherForecast == null)
            {
                Debug.WriteLine("Hourly weather forecast is null");
                continue;
            }

            var windDeg = hourlyWeatherForecast?.WindDeg ?? 0L;
            var windIconId = WindDirection[(int)Math.Round(windDeg / 22.5, 0)];
            var locationDt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
                .AddSeconds(3600 * ((double)km / speed) + issuedForUnixTimestamp).ToLocalTime();

            var locationForecast = new LocationWeatherForecast
            {
                Km = km,
                HourlyWeatherForecast = hourlyWeatherForecast!,
                DateTime = locationDt,
                WeatherIcon = hourlyWeatherForecast!.Icon,
                WindIcon = WindIconsDictionary[windIconId],
                WindIconId = windIconId
            };

            if (weatherForecastProvider == OpenWeather)
            {
                OpenWeatherLocationWeatherForecasts.Add(locationForecast);
            }
            else if (weatherForecastProvider == Yr)
            {
                YrLocationWeatherForecasts.Add(locationForecast);
            }
            else if (weatherForecastProvider == Smhi)
            {
                SmhiLocationWeatherForecasts.Add(locationForecast);
            }
        }
    }
}
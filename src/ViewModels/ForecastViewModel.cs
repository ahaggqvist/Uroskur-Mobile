namespace Uroskur.ViewModels;

[QueryProperty(nameof(ForecastRoute), nameof(ForecastRoute))]
public partial class ForecastViewModel : BaseViewModel
{
    private const int FirstForecastHour = 0;
    private const int LastForecastHour = 47;
    private readonly IOpenWeatherService _openWeatherService;
    private readonly Dictionary<long, string> _weatherIconsDictionary;
    private readonly string[] _windDirection = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
    private readonly Dictionary<string, string> _windIconsDictionary;
    [ObservableProperty] private string? _emptyViewMessage;
    [ObservableProperty] private string? _forecastForDates;
    [ObservableProperty] private ForecastRoute? _forecastRoute;

    public ForecastViewModel(IOpenWeatherService openWeatherService)
    {
        Title = "Forecast Route";

        _openWeatherService = openWeatherService;

        GetForecastCommand = new Command(async () => await GetForecastAsync().ConfigureAwait(false));

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

    public ICommand GetForecastCommand { get; }

    public async Task GetForecastAsync()
    {
        if (_forecastRoute == null)
        {
            return;
        }

        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;

            var today = DateTime.Today;
            if (_forecastRoute is { Day: "TOMORROW" })
            {
                today = today.AddDays(1);
            }

            var hour = _forecastRoute.Time!.Value.Hours;
            var dateTime = today.AddHours(hour).AddMinutes(0).AddSeconds(0).ToLocalTime();
            //var dateTime = new DateTime(2022, 2, 20, 18, 0, 0).ToLocalTime();

            var unixTime = (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var athleteId = _forecastRoute?.Routes?.Athlete?.Id.ToString()!;
            var routeId = _forecastRoute?.Routes?.Id.ToString()!;
            var temperatures = await _openWeatherService.FindForecastAsync(routeId, athleteId);

            var temperaturesEnumerable = temperatures.ToList();
            if (temperaturesEnumerable.Count > 0)
            {
                var temperature = temperaturesEnumerable.ElementAt(0);
                var firstDt = temperature.Hourly![FirstForecastHour].Dt;
                var lastDt = temperature.Hourly[LastForecastHour].Dt;

                var startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds((double)firstDt);
                var endDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds((double)lastDt);

                _forecastForDates = $"Forecast: {startDateTime:ddd, dd MMM HH:mm} - {endDateTime:ddd, dd MMM HH:mm}";

                OnPropertyChanged(nameof(ForecastForDates));

                if (dateTime < startDateTime)
                {
                    _emptyViewMessage =
                        "The start time is before the forecast start time";
                    OnPropertyChanged(nameof(EmptyViewMessage));
                }

                else if (dateTime > startDateTime)
                {
                    _emptyViewMessage =
                        "The start time is after the forecast end time";
                    OnPropertyChanged(nameof(EmptyViewMessage));
                }
                else
                {
                    _emptyViewMessage =
                        "Sorry, we couldn't generate a forecast";
                    OnPropertyChanged(nameof(EmptyViewMessage));
                }
            }

            foreach (var (temperature, index) in temperaturesEnumerable.WithIndex())
            {
                var km = index * 10 + 10;
                var speed = _forecastRoute!.Speed!.Value;
                var time = km / speed;
                var seconds = 3600 * time + unixTime;
                var hourly = temperature.Hourly?.Find(h => h.Dt!.Value == seconds);

                if (hourly == null)
                {
                    continue;
                }

                var weatherIconId = hourly?.Weather?[0].Id;
                var windDeg = hourly?.WindDeg!.Value;
                var windIconId = _windDirection[(int)Math.Round((double)windDeg / 22.5, 0)];
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

    private static DateTime UnixTimeToDateTime(int km, int speed, int unixTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(3600 * ((double)km / speed) + unixTime).ToLocalTime();
    }
}
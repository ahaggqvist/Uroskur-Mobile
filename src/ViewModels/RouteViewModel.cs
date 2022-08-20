namespace Uroskur.ViewModels;

[QueryProperty(nameof(Routes), nameof(Routes))]
public partial class RouteViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    [ObservableProperty] private Routes? _routes;
    [ObservableProperty] private string _selectedDay;
    [ObservableProperty] private string _selectedSpeed;
    [ObservableProperty] private TimeSpan _time;
    [ObservableProperty] private string _weatherForecastProviderName;

    public RouteViewModel(INavigationService navigationService)
    {
        Speeds = Enumeration.GetAll<Speed>().Select(s => s.Name).ToList();
        Days = Enumeration.GetAll<Day>().Select(s => s.Name).ToList();
        WeatherForecastProviders = Enumeration.GetAll<WeatherForecastProvider>().Select(s => s.Name).ToList();

        _navigationService = navigationService;
        _time = new TimeSpan(DateTime.Now.TimeOfDay.Hours, 0, 0);
        _selectedDay = Day.Today.Name;
        _selectedSpeed = Speed.Thirty.Name;
        _weatherForecastProviderName = Yr.Name;
    }

    public List<string> Speeds { get; }
    public List<string> Days { get; }

    public List<string> WeatherForecastProviders { get; }

    [RelayCommand]
    private async void NavigateTo()
    {
        var route = nameof(WeatherForecastPage);
        if (Enumeration.FromName<WeatherForecastProvider>(_weatherForecastProviderName) == Combined)
        {
            route = nameof(CombinedWeatherForecastPage);
        }

        await _navigationService.NavigateToAsync(route, new Dictionary<string, object>
        {
            {
                nameof(WeatherForecastParameters), new WeatherForecastParameters
                {
                    DayId = Enumeration.FromName<Day>(SelectedDay).Id,
                    Time = new TimeSpan(Time.Hours, 0, 0),
                    SpeedId = Enumeration.FromName<Speed>(SelectedSpeed).Id,
                    Routes = Routes,
                    WeatherForecastProviderId = Enumeration.FromName<WeatherForecastProvider>(_weatherForecastProviderName).Id
                }
            }
        });
    }
}
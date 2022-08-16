namespace Uroskur.ViewModels;

[QueryProperty(nameof(Routes), nameof(Routes))]
public partial class RouteViewModel : BaseViewModel
{
    private readonly IRoutingService _routingService;
    [ObservableProperty] private string _day;
    [ObservableProperty] private Routes? _routes;
    [ObservableProperty] private string _speed;
    [ObservableProperty] private TimeSpan _time;
    [ObservableProperty] private string _weatherForecastProviderName;

    public RouteViewModel(IRoutingService routingService)
    {
        _routingService = routingService;

        _time = DateTime.Now.TimeOfDay;
        _day = "Today";
        _speed = "30";
        _weatherForecastProviderName = Yr.Name;
    }

    [RelayCommand]
    private async void NavigateTo()
    {
        await _routingService.NavigateToAsync(nameof(WeatherForecastPage), new Dictionary<string, object>
        {
            {
                nameof(WeatherForecastParameters), new WeatherForecastParameters
                {
                    Day = _day,
                    Time = _time,
                    Speed = int.Parse(_speed),
                    Routes = Routes,
                    WeatherForecastProviderId = Enumeration.FromName<WeatherForecastProvider>(_weatherForecastProviderName).Id
                }
            }
        });
    }
}
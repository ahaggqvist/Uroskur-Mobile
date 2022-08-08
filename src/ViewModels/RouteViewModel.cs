using CommunityToolkit.Mvvm.Input;

namespace Uroskur.ViewModels;

[QueryProperty(nameof(Routes), nameof(Routes))]
public partial class RouteViewModel : BaseViewModel
{
    private const string DefaultDay = "Today";

    private const string DefaultSpeed = "30";

    private readonly IRoutingService _routingService;

    [ObservableProperty] private string _day;

    [ObservableProperty] private Routes? _routes;

    [ObservableProperty] private string _speed;

    [ObservableProperty] private TimeSpan _time;

    public RouteViewModel(IRoutingService routingService)
    {
        _routingService = routingService;

        _time = DateTime.Now.TimeOfDay;
        _day = DefaultDay;
        _speed = DefaultSpeed;
    }

    [RelayCommand]
    private async void NavigateTo()
    {
        var forecast = new ForecastRoute
        {
            Day = _day,
            Time = _time,
            Speed = int.Parse(_speed),
            Routes = Routes
        };

        var dictionary = new Dictionary<string, object>
        {
            { nameof(ForecastRoute), forecast }
        };

        await _routingService.NavigateToAsync(nameof(ForecastPage), dictionary);
    }
}
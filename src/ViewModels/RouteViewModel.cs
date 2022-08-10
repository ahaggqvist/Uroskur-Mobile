using Uroskur.Models.Strava;

namespace Uroskur.ViewModels;

[QueryProperty(nameof(Routes), nameof(Routes))]
public partial class RouteViewModel : BaseViewModel
{
    private readonly IRoutingService _routingService;
    [ObservableProperty] private string _day;
    [ObservableProperty] private string _forecastProvider;
    [ObservableProperty] private Routes? _routes;
    [ObservableProperty] private string _speed;
    [ObservableProperty] private TimeSpan _time;

    public RouteViewModel(IRoutingService routingService)
    {
        _routingService = routingService;

        _time = DateTime.Now.TimeOfDay;
        _day = "Today";
        _speed = "30";
        _forecastProvider = Constants.ForecastProvider.Yr.ToString();
    }

    [RelayCommand]
    private async void NavigateTo()
    {
        await _routingService.NavigateToAsync($"{_forecastProvider}ForecastPage", new Dictionary<string, object>
        {
            {
                nameof(ForecastQuery), new ForecastQuery
                {
                    Day = _day,
                    Time = _time,
                    Speed = int.Parse(_speed),
                    Routes = Routes
                }
            }
        });
    }
}
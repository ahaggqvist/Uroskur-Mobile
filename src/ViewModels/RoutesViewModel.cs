using Uroskur.Models.Strava;

namespace Uroskur.ViewModels;

public class RoutesViewModel : BaseViewModel
{
    private readonly IPreferencesService _preferencesService;
    private readonly IRoutingService _routingService;
    private readonly IStravaService _stravaService;

    public RoutesViewModel(IStravaService stravaService, IRoutingService routingService, IPreferencesService preferencesService)
    {
        Title = "Routes";

        _stravaService = stravaService;
        _routingService = routingService;
        _preferencesService = preferencesService;

        GetRoutesCommand = new Command(async () => await GetRoutesAsync());
    }

    public ObservableCollection<Routes> Routes { get; set; } = new();

    public ICommand GetRoutesCommand { get; }

    public async Task GetRoutesAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;

            var preferences = _preferencesService.FindPreferences();
            var athleteId = preferences.StravaAthleteId;
            var routes = await _stravaService.FindRoutesByAthleteIdAsync(athleteId);

            if (Routes.Count != 0)
            {
                Routes.Clear();
            }

            foreach (var route in routes)
            {
                Routes.Add(route);
            }

            Routes.Sort(c => c.OrderBy(r => r.Name));
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

    public async void NavigateTo(Routes routes)
    {
        await _routingService.NavigateToAsync(nameof(RoutePage), new Dictionary<string, object>
        {
            { nameof(Routes), routes }
        });
    }
}
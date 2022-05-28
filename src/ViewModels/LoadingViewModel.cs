namespace Uroskur.ViewModels;

public class LoadingViewModel : BaseViewModel
{
    private readonly IRoutingService _routingService;

    public LoadingViewModel(IRoutingService routingService)
    {
        Title = "Uroskur";

        _routingService = routingService;
    }

    public async void NavigateTo()
    {
        await _routingService.NavigateToAsync($"//{nameof(RoutesPage)}").ConfigureAwait(false);
    }
}
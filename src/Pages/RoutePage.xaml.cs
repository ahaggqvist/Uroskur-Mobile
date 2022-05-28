namespace Uroskur.Pages;

public partial class RoutePage
{
    public RoutePage(RouteViewModel routeViewModel, IRoutingService routeService)
    {
        InitializeComponent();

        BindingContext = routeViewModel;
    }
}
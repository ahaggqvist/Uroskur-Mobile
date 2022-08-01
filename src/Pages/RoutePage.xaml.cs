namespace Uroskur.Pages;

public partial class RoutePage
{
    public RoutePage(RouteViewModel routeViewModel)
    {
        InitializeComponent();

        BindingContext = routeViewModel;
    }
}
namespace Uroskur.Pages;

public partial class RoutesPage
{
    private readonly RoutesViewModel _routesViewModel;

    public RoutesPage(RoutesViewModel routesViewModel)
    {
        InitializeComponent();

        BindingContext = _routesViewModel = routesViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadRoutes();
    }

    private async Task LoadRoutes()
    {
        ShowIndicator();

        await _routesViewModel.RoutesAsync();

        EmptyRoutesMessage.Text = "Sorry, We Couldn't Find any Routes";

        HideIndicator();
    }

    private void ShowIndicator()
    {
        Indicator.IsRunning = true;
        Indicator.IsVisible = true;
    }

    private void HideIndicator()
    {
        Indicator.IsRunning = false;
        Indicator.IsVisible = false;
    }
}
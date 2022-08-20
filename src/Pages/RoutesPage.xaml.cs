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

        await _routesViewModel.RoutesAsync();
        ActivityIndicator.IsVisible = _routesViewModel.IsBusy;

        EmptyRoutesMessage.Text = "Sorry, We Couldn't Find any Routes";
    }
}
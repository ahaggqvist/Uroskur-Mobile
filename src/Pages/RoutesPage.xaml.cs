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

        await Task.Delay(500);

        if (_routesViewModel.Routes.Count == 0)
        {
            await _routesViewModel.GetRoutesAsync();
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        if (((VisualElement)sender).BindingContext is not Routes routes)
        {
            return;
        }

        _routesViewModel.NavigateTo(routes);
    }
}
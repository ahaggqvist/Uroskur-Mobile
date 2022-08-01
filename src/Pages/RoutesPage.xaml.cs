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

        if (_routesViewModel.Routes.Count != 0)
        {
            return;
        }

        await _routesViewModel.GetRoutesAsync();

        emptyRoutesLabel.Text = "Sorry, We Couldn't Find any Routes";
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
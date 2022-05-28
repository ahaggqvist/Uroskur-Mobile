namespace Uroskur.Pages;

public partial class ForecastPage
{
    private readonly ForecastViewModel _forecastViewModel;

    public ForecastPage(ForecastViewModel forecastViewModel)
    {
        InitializeComponent();

        BindingContext = _forecastViewModel = forecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(1000);

        if (_forecastViewModel.Forecasts.Count == 0)
        {
            await _forecastViewModel.GetForecastAsync();
        }
    }
}
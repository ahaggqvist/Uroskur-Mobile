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

        ActivityIndicator(true, true);

        await Task.Delay(1000);

        if (_forecastViewModel.Forecasts.Count != 0)
        {
            return;
        }

        await _forecastViewModel.GetForecastAsync();

        temperatureChartViewLabel.Text = "Temperatures";
        chanceOfRainChartViewLabel.Text = "Chance of Rain and Cloudiness";

        ActivityIndicator(false, false);
    }

    private void ActivityIndicator(bool isRunning, bool isVisible)
    {
        activityIndicator.IsRunning = isRunning;
        activityIndicator.IsVisible = isVisible;
    }
}
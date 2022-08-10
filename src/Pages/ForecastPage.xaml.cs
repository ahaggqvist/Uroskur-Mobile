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

        if (_forecastViewModel.LocationForecasts.Count != 0)
        {
            return;
        }

        await _forecastViewModel.GetForecastAsync();

        ChartViewTemperatureLabel.Text = "Temperatures";
        ChartViewChanceOfRainLabel.Text = "Chance of Rain and Cloudiness";

        TableTempLabel.Text = "Temp °C";
        TableTimeLabel.Text = "Time";
        TableFeelsLikeLabel.Text = "Feels Like °C";
        TableChanceOfRainLabel.Text = "Chance of Rain";
        TableWindLabel.Text = "Wind m/s";

        ActivityIndicator(false, false);
    }

    private void ActivityIndicator(bool isRunning, bool isVisible)
    {
        ActivityIndicatorStatus.IsRunning = isRunning;
        ActivityIndicatorStatus.IsVisible = isVisible;
    }
}
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

        chartViewTemperatureLabel.Text = "Temperatures";
        chartViewChanceOfRainLabel.Text = "Chance of Rain and Cloudiness";

        tableTempLabel.Text = "Temp �C";
        tableTimeLabel.Text = "Time";
        tableFeelsLikeLabel.Text = "Feels Like �C";
        tableChanceOfRainLabel.Text = "Chance of Rain";
        tableWindLabel.Text = "Wind m/s";

        ActivityIndicator(false, false);
    }

    private void ActivityIndicator(bool isRunning, bool isVisible)
    {
        activityIndicator.IsRunning = isRunning;
        activityIndicator.IsVisible = isVisible;
    }
}
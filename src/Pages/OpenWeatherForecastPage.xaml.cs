namespace Uroskur.Pages;

public partial class OpenWeatherForecastPage
{
    private readonly OpenWeatherForecastViewModel _openWeatherForecastViewModel;

    public OpenWeatherForecastPage(OpenWeatherForecastViewModel openWeatherForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _openWeatherForecastViewModel = openWeatherForecastViewModel;

        Debug.WriteLine($"The URI of the current page: {Shell.Current.CurrentState.Location}.");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        ActivityIndicator(true, true);

        await Task.Delay(500);

        if (_openWeatherForecastViewModel.LocationForecasts.Count != 0)
        {
            ActivityIndicator(false, false);
            return;
        }

        await _openWeatherForecastViewModel.GetForecastAsync();

        ChartViewTemperatureLabel.Text = "Temperatures";
        ChartViewChanceOfRainLabel.Text = "Chance of Rain";
        ChartViewUvLabel.Text = "UV index and Cloudiness";
        ChartViewWindLabel.Text = "Wind Speed and Wind Gust";

        TableTempLabel.Text = "Temp �C";
        TableTimeLabel.Text = "Time";
        TableFeelsLikeLabel.Text = "Feels Like �C";
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
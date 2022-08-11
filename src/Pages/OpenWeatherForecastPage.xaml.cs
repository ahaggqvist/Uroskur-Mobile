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

        ChartTemperatureHeader.Text = "Temperatures";
        ChartChanceOfRainHeader.Text = "Chance of Rain";
        ChartUvHeader.Text = "UV index and Cloudiness";
        ChartWindHeader.Text = "Wind Speed and Wind Gust";

        WeatherTableTempHeader.Text = "Temp °C";
        WaeatheTableTimeHeader.Text = "Time";
        WeathTableFeelsLikeHeader.Text = "Feels Like °C";
        WeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        WeatherTableWindHeader.Text = "Wind m/s";

        ActivityIndicator(false, false);
    }

    private void ActivityIndicator(bool isRunning, bool isVisible)
    {
        ActivityIndicatorStatus.IsRunning = isRunning;
        ActivityIndicatorStatus.IsVisible = isVisible;
    }
}
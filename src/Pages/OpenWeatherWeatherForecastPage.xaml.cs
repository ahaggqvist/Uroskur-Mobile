namespace Uroskur.Pages;

public partial class OpenWeatherWeatherForecastPage
{
    private readonly OpenWeatherForecastViewModel _openWeatherForecastViewModel;

    public OpenWeatherWeatherForecastPage(OpenWeatherForecastViewModel openWeatherForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _openWeatherForecastViewModel = openWeatherForecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadWeatherForecast();
    }

    private async Task LoadWeatherForecast()
    {
        ShowIndicator();

        await _openWeatherForecastViewModel.WeatherForecastAsync();

        EmptyWeatherForecastMessage.Text = "Sorry, We Couldn't Generate a Forecast";

        ChartTemperatureHeader.Text = "Temperatures";
        ChartChanceOfRainHeader.Text = "Chance of Rain";
        ChartUvHeader.Text = "UV index and Cloudiness";
        ChartWindHeader.Text = "Wind Speed and Wind Gust";

        WeatherTableTempHeader.Text = "Temp °C";
        WaeatheTableTimeHeader.Text = "Time";
        WeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        WeatherTableWindHeader.Text = "Wind m/s";

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
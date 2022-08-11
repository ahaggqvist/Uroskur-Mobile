namespace Uroskur.Pages;

public partial class OpenWeatherForecastPage
{
    private readonly OpenWeatherForecastViewModel _openWeatherForecastViewModel;

    public OpenWeatherForecastPage(OpenWeatherForecastViewModel openWeatherForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _openWeatherForecastViewModel = openWeatherForecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Run(async () => await _openWeatherForecastViewModel.WeatherForecastAsync());

        ChartTemperatureHeader.Text = "Temperatures";
        ChartChanceOfRainHeader.Text = "Chance of Rain";
        ChartUvHeader.Text = "UV index and Cloudiness";
        ChartWindHeader.Text = "Wind Speed and Wind Gust";

        WeatherTableTempHeader.Text = "Temp °C";
        WaeatheTableTimeHeader.Text = "Time";
        WeathTableFeelsLikeHeader.Text = "Feels Like °C";
        WeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        WeatherTableWindHeader.Text = "Wind m/s";
    }
}
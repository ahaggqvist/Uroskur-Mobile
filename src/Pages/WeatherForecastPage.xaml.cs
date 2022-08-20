namespace Uroskur.Pages;

public partial class WeatherForecastPage
{
    private readonly WeatherForecastViewModel _weatherForecastViewModel;

    public WeatherForecastPage(WeatherForecastViewModel weatherForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _weatherForecastViewModel = weatherForecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _weatherForecastViewModel.WeatherForecastAsync();
        ActivityIndicator.IsVisible = _weatherForecastViewModel.IsBusy;

        EmptyWeatherForecastMessage.Text = "Sorry, We Couldn't Generate a Forecast";

        ChartTemperatureHeader.Text = "Temperature";
        ChartChanceOfRainHeader.Text = "Chance of Rain and Rain";
        ChartUvHeader.Text = "UV index and Cloudiness";
        ChartWindHeader.Text = "Wind Speed and Wind Gust";

        WeatherTableTempHeader.Text = "Temp (°C)";
        WeatherTablePrecipitationAmountHeader.Text = "Rain (mm)";
        WeatherTableTimeHeader.Text = "";
        WeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        WeatherTableWindHeader.Text = "Wind (m/s)";
    }
}
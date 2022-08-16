namespace Uroskur.Pages;

public partial class YrWeatherForecastPage
{
    private readonly WeatherForecastViewModel _weatherForecastViewModel;

    public YrWeatherForecastPage(WeatherForecastViewModel weatherForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _weatherForecastViewModel = weatherForecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadWeatherForecast();
    }

    private async Task LoadWeatherForecast()
    {
        ShowIndicator();

        await _weatherForecastViewModel.WeatherForecastAsync();

        EmptyWeatherForecastMessage.Text = "Sorry, We Couldn't Generate a Forecast";

        ChartTemperatureHeader.Text = "Temperature";
        ChartChanceOfRainHeader.Text = "Chance of Rain and amount of Rain";
        ChartUvHeader.Text = "UV index and Cloudiness";
        ChartWindHeader.Text = "Wind Speed and Wind Gust";

        WeatherTableTempHeader.Text = "Temp °C";
        WeatherTableTimeHeader.Text = "";
        WeatherTablePrecipitationAmountHeader.Text = "Rain mm";
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
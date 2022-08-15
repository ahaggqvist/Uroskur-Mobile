namespace Uroskur.Pages;

public partial class YrWeatherForecastPage
{
    private readonly YrWeatherForecastViewModel _yrWeatherForecastViewModel;

    public YrWeatherForecastPage(YrWeatherForecastViewModel yrWeatherForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _yrWeatherForecastViewModel = yrWeatherForecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadWeatherForecast();
    }

    private async Task LoadWeatherForecast()
    {
        ShowIndicator();

        await _yrWeatherForecastViewModel.WeatherForecastAsync();

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
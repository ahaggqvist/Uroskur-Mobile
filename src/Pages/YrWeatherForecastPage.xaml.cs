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
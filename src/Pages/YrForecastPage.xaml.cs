namespace Uroskur.Pages;

public partial class YrForecastPage
{
    private readonly YrForecastViewModel _yrForecastViewModel;

    public YrForecastPage(YrForecastViewModel yrForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _yrForecastViewModel = yrForecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Run(async () => await _yrForecastViewModel.WeatherForecastAsync());

        ChartTemperatureHeader.Text = "Temperatures";
        ChartChanceOfRainHeader.Text = "Chance of Rain";
        ChartUvHeader.Text = "UV index and Cloudiness";
        ChartWindHeader.Text = "Wind Speed and Wind Gust";

        WeatherTableTempHeader.Text = "Temp °C";
        WaeatheTableTimeHeader.Text = "Time";
        WeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        WeatherTableWindHeader.Text = "Wind m/s";
    }
}
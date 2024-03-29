namespace Uroskur.Pages;

public partial class CombinedWeatherForecastPage
{
    private readonly CombinedWeatherForecastViewModel _combinedWeatherForecastViewModel;

    public CombinedWeatherForecastPage(CombinedWeatherForecastViewModel combinedWeatherForecastViewModel)
    {
        InitializeComponent();

        BindingContext = _combinedWeatherForecastViewModel = combinedWeatherForecastViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _combinedWeatherForecastViewModel.WeatherForecastAsync();
        ActivityIndicator.IsVisible = _combinedWeatherForecastViewModel.IsBusy;

        EmptyWeatherForecastMessage.Text = "Sorry, We Couldn't Generate a Forecast";
        WeatherTableTempHeader.Text = "Temp (°C)";
        WeatherTablePrecipitationAmountHeader.Text = "Rain (mm)";
        WeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        WeatherTableWindHeader.Text = "Wind (m/s)";
        WeatherTableHeader.Text = "OpenWeather";

        YrEmptyWeatherForecastMessage.Text = "Sorry, We Couldn't Generate a Forecast";
        YrWeatherTableTempHeader.Text = "Temp °C";
        YrWeatherTablePrecipitationAmountHeader.Text = "Rain (mm)";
        YrWeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        YrWeatherTableWindHeader.Text = "Wind (m/s)";
        YrWeatherTableHeader.Text = "Yr";

        SmhiEmptyWeatherForecastMessage.Text = "Sorry, We Couldn't Generate a Forecast";
        SmhiWeatherTableTempHeader.Text = "Temp (°C)";
        SmhiWeatherTablePrecipitationAmountHeader.Text = "Rain (mm)";
        SmhiWeatherTableChanceOfRainHeader.Text = "Chance of Rain";
        SmhiWeatherTableWindHeader.Text = "Wind (m/s)";
        SmhiWeatherTableHeader.Text = "SMHI";
    }
}
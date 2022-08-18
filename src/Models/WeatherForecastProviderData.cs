namespace Uroskur.Models;

public class WeatherForecastProviderData
{
    public WeatherForecastProviderData(OpenWeatherData openWeatherData)
    {
        OpenWeatherData = openWeatherData;
    }

    public WeatherForecastProviderData(SmhiData smhiData)
    {
        SmhiData = smhiData;
    }

    public WeatherForecastProviderData(YrData yrData)
    {
        YrData = yrData;
    }

    public OpenWeatherData? OpenWeatherData { get; init; }
    public SmhiData? SmhiData { get; init; }
    public YrData? YrData { get; init; }
}
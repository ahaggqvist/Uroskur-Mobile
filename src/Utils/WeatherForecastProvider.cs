namespace Uroskur.Utils;

public class WeatherForecastProvider : Enumeration
{
    public static readonly WeatherForecastProvider OpenWeather = new(1, "OpenWeather");
    public static readonly WeatherForecastProvider Yr = new(2, "Yr");
    public static readonly WeatherForecastProvider Smhi = new(3, "SMHI");
    public static readonly WeatherForecastProvider Combined = new(4, "OpenWeather/Yr/SMHI");

    public WeatherForecastProvider()
    {
    }

    public WeatherForecastProvider(int id, string name)
        : base(id, name)
    {
    }
}
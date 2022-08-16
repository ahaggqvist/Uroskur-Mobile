namespace Uroskur.Utils;

public class WeatherForecastProvider : Enumeration
{
    public static readonly WeatherForecastProvider OpenWeather = new(1, nameof(OpenWeather));
    public static readonly WeatherForecastProvider Yr = new(2, nameof(Yr));
    public static readonly WeatherForecastProvider Smhi = new(3, nameof(Smhi));

    public WeatherForecastProvider()
    {
    }

    public WeatherForecastProvider(int id, string name)
        : base(id, name)
    {
    }
}
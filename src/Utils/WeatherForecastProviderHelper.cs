namespace Uroskur.Utils;

public static class WeatherForecastProviderHelper
{
    public static WeatherForecastProvider? ResolveWeatherForecastProviderByUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return null;
        if (url.Contains("openweathermap.org", StringComparison.InvariantCultureIgnoreCase)) return OpenWeather;
        if (url.Contains("met.no", StringComparison.InvariantCultureIgnoreCase)) return Yr;
        return url.Contains("smhi.se", StringComparison.InvariantCultureIgnoreCase) ? Smhi : null;
    }
}
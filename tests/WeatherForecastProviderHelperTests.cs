namespace Uroskur.Tests;

public class WeatherForecastProviderHelperTests
{
    private const string OpenWeatherApiUrl = "https://api.openweathermap.org/";
    private const string YrApiUrl = "https://api.met.no/weatherapi/";
    private const string SmhiApiUrl = "https://opendata-download-metfcst.smhi.se/";

    [InlineData(null), InlineData(""), Theory]
    public void Resolve_provider_as_invalid_by_url(string url)
    {
        Assert.Null(WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(url));
        Assert.Null(WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(url));
    }

    [Fact]
    public void Resolve_provider_as_openweather_by_url()
    {
        Assert.Equal(OpenWeather, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(OpenWeatherApiUrl));
        Assert.NotEqual(Yr, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(OpenWeatherApiUrl));
        Assert.NotEqual(Smhi, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(OpenWeatherApiUrl));
    }

    [Fact]
    public void Resolve_provider_as_yr_by_url()
    {
        Assert.Equal(Yr, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(YrApiUrl));
        Assert.NotEqual(OpenWeather, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(YrApiUrl));
        Assert.NotEqual(Smhi, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(YrApiUrl));
    }

    [Fact]
    public void Resolve_provider_as_smhi_by_url()
    {
        Assert.Equal(Smhi, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(SmhiApiUrl));
        Assert.NotEqual(OpenWeather, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(SmhiApiUrl));
        Assert.NotEqual(Yr, WeatherForecastProviderHelper.ResolveWeatherForecastProviderByUrl(SmhiApiUrl));
    }
}
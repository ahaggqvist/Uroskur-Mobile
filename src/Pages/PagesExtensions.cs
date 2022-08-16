namespace Uroskur.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<RoutePage>();
        builder.Services.AddTransient<RoutesPage>();
        builder.Services.AddTransient<PreferencesPage>();
        builder.Services.AddTransient<WeatherForecastPage>();
        builder.Services.AddTransient<CombinedWeatherForecastPage>();
        builder.Services.AddTransient<AboutPage>();
        return builder;
    }
}
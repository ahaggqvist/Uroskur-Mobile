namespace Uroskur.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddTransient<RoutePage>();
        builder.Services.AddTransient<RoutesPage>();
        builder.Services.AddTransient<PreferencesPage>();
        builder.Services.AddTransient<ForecastPage>();
        builder.Services.AddTransient<AboutPage>();
        return builder;
    }
}
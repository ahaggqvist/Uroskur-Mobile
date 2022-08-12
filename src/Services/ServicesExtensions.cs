namespace Uroskur.Services;

public static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IRoutingService, RoutingService>();
        builder.Services.AddScoped<IPreferencesService, PreferencesService>();
        builder.Services.AddScoped<IStravaService, StravaService>();
        builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        return builder;
    }
}
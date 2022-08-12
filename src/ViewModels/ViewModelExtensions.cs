namespace Uroskur.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<RoutesViewModel>();
        builder.Services.AddTransient<RouteViewModel>();
        builder.Services.AddTransient<OpenWeatherForecastViewModel>();
        builder.Services.AddTransient<YrWeatherForecastViewModel>();
        builder.Services.AddTransient<PreferencesViewModel>();
        builder.Services.AddTransient<AboutViewModel>();
        return builder;
    }
}
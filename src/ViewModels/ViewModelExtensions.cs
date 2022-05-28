namespace Uroskur.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoadingViewModel>();
        builder.Services.AddTransient<RoutesViewModel>();
        builder.Services.AddTransient<RouteViewModel>();
        builder.Services.AddTransient<ForecastViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<AboutViewModel>();

        return builder;
    }
}
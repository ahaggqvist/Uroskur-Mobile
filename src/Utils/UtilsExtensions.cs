namespace Uroskur.Utils;

public static class UtilsExtensions
{
    public static MauiAppBuilder ConfigureUtils(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(provider => new HttpClient());
        builder.Services.AddScoped<IStravaClient>(provider => new StravaClient(provider.GetService<AppSettings>(),
            provider.GetService<HttpClient>()));
        builder.Services.AddScoped<IOpenWeatherClient>(provider => new OpenWeatherClient(
            provider.GetService<AppSettings>(),
            provider.GetService<HttpClient>()));
        builder.Services.AddScoped<IYrClient>(provider => new YrClient(provider.GetService<AppSettings>(),
            provider.GetService<HttpClient>()));
        builder.Services.AddScoped<ISmhiClient>(provider => new SmhiClient(provider.GetService<AppSettings>(),
            provider.GetService<HttpClient>()));
        return builder;
    }
}
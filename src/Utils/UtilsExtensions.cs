﻿namespace Uroskur.Utils;

public static class UtilsExtensions
{
    public static MauiAppBuilder ConfigureUtils(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(_ => new HttpClient());
        builder.Services.AddScoped<IStravaClient>(provider => new StravaClient(provider.GetService<AppSettings>(),
            provider.GetService<HttpClient>()));
        builder.Services.AddScoped<IWeatherForecastClient>(provider => new WeatherForecastClient(
            provider.GetService<HttpClient>()));
        return builder;
    }
}
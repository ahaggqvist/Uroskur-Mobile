using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Uroskur;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>().UseSkiaSharp().ConfigureServices().ConfigurePages().ConfigureViewModels().ConfigureUtils()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Light.ttf", "RobotoLight");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                fonts.AddFont("weathericons-regular-webfont.ttf", "Weathericons");
            });

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Uroskur.appsettings.json");
        if (stream != null)
        {
            var serializer = new JsonSerializer();
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var appSettings = serializer.Deserialize<AppSettings>(jsonTextReader)!;
            builder.Services.AddSingleton(appSettings);
        }

        Routing.RegisterRoute(nameof(RoutePage), typeof(RoutePage));
        Routing.RegisterRoute(nameof(ForecastPage), typeof(ForecastPage));

        Barrel.ApplicationId = "uroskur.700c.se";

        return builder.Build();
    }
}
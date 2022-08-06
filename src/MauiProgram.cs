using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Uroskur;

public static class MauiProgram
{
    private const bool IsDevelopment = false;
    private const string AppSettingsProduction = "appsettings.json";
    private const string AppSettingsDevelopment = "appsettings.Development.json";

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>().UseSkiaSharp().ConfigureServices().ConfigurePages().ConfigureViewModels()
            .ConfigureUtils()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Light.ttf", "RobotoLight");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                fonts.AddFont("weathericons-regular-webfont.ttf", "Weathericons");
            });

        const string manifestFileName = IsDevelopment ? AppSettingsDevelopment : AppSettingsProduction;
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"Uroskur.{manifestFileName}");
        if (stream != null)
        {
            var serializer = new JsonSerializer();
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var appSettings = serializer.Deserialize<AppSettings>(jsonTextReader)!;
            appSettings.IsDevelopment = IsDevelopment;
            builder.Services.AddSingleton(appSettings);
        }

        Routing.RegisterRoute(nameof(RoutePage), typeof(RoutePage));
        Routing.RegisterRoute(nameof(ForecastPage), typeof(ForecastPage));

        Barrel.ApplicationId = "uroskur.pii.at";

        return builder.Build();
    }
}
namespace Uroskur;

public static class MauiProgram
{
#if DEBUG
    private const string ManifestFileName = "appsettings.Development.json";
#else
    private const string ManifestFileName = "appsettings.json";
#endif

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>().UseMauiCommunityToolkit().UseSkiaSharp().ConfigureServices().ConfigurePages().ConfigureViewModels()
            .ConfigureUtils()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Light.ttf", "RobotoLight");
                fonts.AddFont("OpenSans-Regular.ttf", "RobotoRegular");
                fonts.AddFont("OpenSans-Medium.ttf", "RobotoMedium");
                fonts.AddFont("OpenSans-Bold.ttf", "RobotoBold");
                fonts.AddFont("weathericons-regular-webfont.ttf", "Weathericons");
            });

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"Uroskur.{ManifestFileName}");
        if (stream != null)
        {
            var serializer = new JsonSerializer();
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var appSettings = serializer.Deserialize<AppSettings>(jsonTextReader)!;
            builder.Services.AddSingleton(appSettings);
        }

        Routing.RegisterRoute(nameof(RoutePage), typeof(RoutePage));
        Routing.RegisterRoute(nameof(WeatherForecastPage), typeof(WeatherForecastPage));
        Routing.RegisterRoute(nameof(CombinedWeatherForecastPage), typeof(CombinedWeatherForecastPage));

        Barrel.ApplicationId = "uroskur.pii.at";

        return builder.Build();
    }
}
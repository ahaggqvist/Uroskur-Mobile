namespace Uroskur.Utils;

public static class Constants
{
    public enum ForecastProvider
    {
        OpenWeather,
        Yr
    }

    public const double ChartHeadingFontSize = 13;
    public const double WeatherIconFontSize = 14;
    public const double WeatherTableHeadingFontSize = 10;
    public const double ShellTitleViewHeadingFontSize = 20;
    public const double ForecastIssuedAtFontSize = 10;
    public const double ForecastIssuedForFontSize = 16;
    public const string ChartHeadingFontFamily = "RobotoMedium";
    public const string ShellTitleViewHeadingFontFamily = "RobotoMedium";
    public const string WeatherTableHeadingFontFamily = "RobotoLight";
    public const string ForecastIssuedAtFontFamily = "RobotoLight";
    public const string ForecastIssuedForFontFamily = "RobotoBold";

    internal static readonly Dictionary<long, string> OpenWeatherIconsDictionary = new()
    {
        { 200, "\uf01e" },
        { 201, "\uf01e" },
        { 202, "\uf01e" },
        { 210, "\uf016" },
        { 211, "\uf016" },
        { 212, "\uf016" },
        { 221, "\uf016" },
        { 230, "\uf01e" },
        { 231, "\uf01e" },
        { 232, "\uf01e" },
        { 300, "\uf01c" },
        { 301, "\uf01c" },
        { 302, "\uf019" },
        { 310, "\uf017" },
        { 311, "\uf019" },
        { 312, "\uf019" },
        { 313, "\uf01a" },
        { 314, "\uf019" },
        { 321, "\uf01c" },
        { 500, "\uf01c" },
        { 501, "\uf019" },
        { 502, "\uf019" },
        { 503, "\uf019" },
        { 504, "\uf019" },
        { 511, "\uf017" },
        { 520, "\uf01a" },
        { 521, "\uf01a" },
        { 522, "\uf01a" },
        { 531, "\uf01d" },
        { 600, "\uf01b" },
        { 601, "\uf01b" },
        { 602, "\uf0b5" },
        { 611, "\uf017" },
        { 612, "\uf017" },
        { 615, "\uf017" },
        { 616, "\uf017" },
        { 620, "\uf017" },
        { 621, "\uf01b" },
        { 622, "\uf01b" },
        { 701, "\uf01a" },
        { 711, "\uf062" },
        { 721, "\uf0b6" },
        { 731, "\uf063" },
        { 741, "\uf014" },
        { 761, "\uf063" },
        { 762, "\uf063" },
        { 771, "\uf011" },
        { 781, "\uf056" },
        { 800, "\uf00d" },
        { 801, "\uf011" },
        { 802, "\uf011" },
        { 803, "\uf012" },
        { 804, "\uf013" },
        { 901, "\uf01d" },
        { 902, "\uf073" },
        { 903, "\uf076" },
        { 904, "\uf072" },
        { 905, "\uf021" },
        { 906, "\uf015" },
        { 957, "\uf050" }
    };

    internal static readonly Dictionary<string, string> YrIconsDictionary = new()
    {
        { "clearsky", "\uf00d" },
        { "clearsky_day", "\uf00d" },
        { "clearsky_night", "\uf02e" },
        { "fair", "\uf002" },
        { "fair_day", "\uf002" },
        { "fair_night", "\uf086" },
        { "partlycloudy", "\uf041" },
        { "partlycloudy_day", "\uf002" },
        { "partlycloudy_night", "\uf083" },
        { "cloudy", "\uf013" },
        { "rainshowers", "\uf01a" },
        { "rainshowers_day", "\uf009" },
        { "rainshowers_night", "\uf029" },
        { "rainandthunder", "\uf01d" },
        { "rainshowersandthunder", "\uf01d" },
        { "rainshowersandthunder_day", "\uf00e" },
        { "rainshowersandthunder_night", "\uf03a" },
        { "sleetshowers_day", "\uf0b2" },
        { "sleetshowers_night", "\uf0b3" },
        { "snowshowers_day", "\uf00a" },
        { "snowshowers_night", "\uf02a" },
        { "sleetshowersandthunder_day", "\uf068" },
        { "sleetshowersandthunder_night", "\uf069" },
        { "snowshowersandthunder_day", "\uf06b" },
        { "snowshowersandthunder_night", "\uf06c" },
        { "lightrainshowersandthunder_day", "\uf00e" },
        { "lightrainshowersandthunder_night", "\uf03a" },
        { "heavyrainshowersandthunder_day", "\uf010" },
        { "heavyrainshowersandthunder_night", "\uf03b" },
        { "lightssleetshowersandthunder_day", "\uf068" },
        { "lightssleetshowersandthunder_night", "\uf069" },
        { "heavysleetshowersandthunder_day", "\uf068" },
        { "heavysleetshowersandthunder_night", "\uf069" },
        { "lightssnowshowersandthunder_day", "\uf06b" },
        { "lightssnowshowersandthunder_night", "\uf06c" },
        { "heavysnowshowersandthunder_day", "\uf06b" },
        { "heavysnowshowersandthunder_night", "\uf06c" },
        { "lightrainshowers_day", "\uf009" },
        { "lightrainshowers_night", "\uf037" },
        { "heavyrainshowers_day", "\uf008" },
        { "heavyrainshowers_night", "\uf036" },
        { "lightsleetshowers_day", "\uf0b2" },
        { "lightsleetshowers_night", "\uf0b3" },
        { "heavysleetshowers_day", "\uf0b2" },
        { "heavysleetshowers_night", "\uf0b3" },
        { "lightsnowshowers_day", "\uf00a" },
        { "lightsnowshowers_night", "\uf038" },
        { "heavysnowshowers_day", "\uf00a" },
        { "heavysnowshowers_night", "\uf038" },
        { "fog", "\uf014" },
        { "heavyrain", "\uf019" },
        { "heavyrainandthunder", "\uf01d" },
        { "heavyrainshowers", "\uf019" },
        { "heavyrainshowersandthunder", "\uf01d" },
        { "heavysleet", "\uf0b5" },
        { "heavysleetandthunder", "\uf068" },
        { "heavysleetshowers", "\uf0b2" },
        { "heavysleetshowersandthunder", "\uf068" },
        { "heavysnow", "\uf01b" },
        { "heavysnowandthunder", "\uf06b" },
        { "heavysnowshowers", "\uf00a" },
        { "heavysnowshowersandthunder", "\uf06b" },
        { "lightrain", "\uf01a" },
        { "lightrainandthunder", "\uf01d" },
        { "lightrainshowers", "\uf009" },
        { "lightrainshowersandthunder", "\uf00e" },
        { "lightsleet", "\uf0b5" },
        { "lightsleetandthunder", "\uf068" },
        { "lightsleetshowers", "\uf0b2" },
        { "lightsnow", "\uf01b" },
        { "lightsnowandthunder", "\uf06b" },
        { "lightsnowshowers", "\uf00a" },
        { "lightssleetshowersandthunder", "\uf068" },
        { "lightssnowshowersandthunder", "\uf06b" },
        { "rain", "\uf019" },
        { "sleet", "\uf0b5" },
        { "sleetandthunder", "\uf068" },
        { "sleetshowers", "\uf0b2" },
        { "sleetshowersandthunder", "\uf068" },
        { "snow", "\uf01b" },
        { "snowandthunder", "\uf06b" },
        { "snowshowers", "\uf00a" },
        { "snowshowersandthunder", "\uf06b" }
    };

    internal static readonly Dictionary<string, string> WindIconsDictionary = new()
    {
        { "N", "\uf058" },
        { "NNE", "\uf057" },
        { "NE", "\uf057" },
        { "ENE", "\uf057" },
        { "E", "\uf04d" },
        { "ESE", "\uf088" },
        { "SE", "\uf088" },
        { "SSE", "\uf088" },
        { "S", "\uf044" },
        { "SSW", "\uf043" },
        { "SW", "\uf043" },
        { "WSW", "\uf043" },
        { "W", "\uf048" },
        { "WNW", "\uf087" },
        { "NW", "\uf087" },
        { "NNW", "\uf087" }
    };

    internal static readonly string[] WindDirection =
        { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
}
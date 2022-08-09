namespace Uroskur.Utils;

public static class Constants
{
    public enum ForecastProvider
    {
        OpenWeather,
        Yr
    }

    internal static readonly Dictionary<long, string> WeatherIconsDictionary = new()
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
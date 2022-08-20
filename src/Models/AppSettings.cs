namespace Uroskur.Models;

public record AppSettings(bool IsDevelopment,
    string StravaAuthorizationTokenMobileUrl,
    string StravaAuthorizationTokenUrl,
    string StravaAuthorizationRedirectUrl,
    string StravaRoutesUrl,
    string StravaGxpUrl,
    string StravaCallbackUrl,
    string OpenWeatherApiUrl,
    string YrApiUrl,
    string SmhiApiUrl);
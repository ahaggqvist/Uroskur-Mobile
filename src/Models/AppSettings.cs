namespace Uroskur.Models;

public class AppSettings
{
    public string StravaAuthorizationTokenMobileUrl { get; set; } = string.Empty;

    public string StravaAuthorizationTokenUrl { get; set; } = string.Empty;

    public string StravaAuthorizationRedirectUrl { get; set; } = string.Empty;

    public string StravaSubscriptionUrl { get; set; } = string.Empty;

    public string StravaRoutesUrl { get; set; } = string.Empty;

    public string StravaGxpUrl { get; set; } = string.Empty;

    public string StravaCallbackUrl { get; set; } = string.Empty;

    public string ForecastUrl { get; set; } = string.Empty;

    public override string ToString()
    {
        return
            $"{nameof(StravaAuthorizationTokenUrl)}: {StravaAuthorizationTokenUrl}, {nameof(StravaAuthorizationRedirectUrl)}: {StravaAuthorizationRedirectUrl}, {nameof(StravaSubscriptionUrl)}: {StravaSubscriptionUrl}, {nameof(StravaRoutesUrl)}: {StravaRoutesUrl}, {nameof(StravaGxpUrl)}: {StravaGxpUrl}, {nameof(StravaCallbackUrl)}: {StravaCallbackUrl}, {nameof(ForecastUrl)}: {ForecastUrl}";
    }
}
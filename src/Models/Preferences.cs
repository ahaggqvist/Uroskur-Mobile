namespace Uroskur.Models;

public class Preferences
{
    public string OpenWeatherAppId { get; set; } = string.Empty;
    public string StravaClientId { get; set; } = string.Empty;
    public string StravaClientSecret { get; set; } = string.Empty;
    public string StravaAthleteId { get; set; } = string.Empty;
    public string StravaAccessToken { get; set; } = string.Empty;
    public string StravaExpiresAt { get; set; } = string.Empty;
    public string StravaFirstname { get; set; } = string.Empty;
    public string StravaLastname { get; set; } = string.Empty;
    public string StravaRefreshToken { get; set; } = string.Empty;
    public string StravaUsername { get; set; } = string.Empty;

    public string StravaExpiresAtFormatted()
    {
        if (!long.TryParse(StravaExpiresAt, out var seconds))
        {
            return string.Empty;
        }

        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).ToLocalTime();
        return $"{dateTime:ddd, d MMM yyyy H:mm} ({StravaExpiresAt})";
    }

    public override string ToString()
    {
        return
            $"{nameof(OpenWeatherAppId)}: {OpenWeatherAppId}, {nameof(StravaClientId)}: {StravaClientId}, {nameof(StravaClientSecret)}: {StravaClientSecret}, {nameof(StravaAthleteId)}: {StravaAthleteId}, {nameof(StravaAccessToken)}: {StravaAccessToken}, {nameof(StravaExpiresAt)}: {StravaExpiresAt}, {nameof(StravaFirstname)}: {StravaFirstname}, {nameof(StravaLastname)}: {StravaLastname}, {nameof(StravaRefreshToken)}: {StravaRefreshToken}, {nameof(StravaUsername)}: {StravaUsername}";
    }
}
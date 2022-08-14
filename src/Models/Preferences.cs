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

        var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
        return $"{dt:dddd, dd MMMM yyyy HH:mm} ({StravaExpiresAt})";
    }
}
namespace Uroskur.Models;

public class AppPreferences
{
    private string? _openWeatherAppId;
    private string? _stravaAccessToken;
    private string? _stravaAthleteId;
    private string? _stravaClientId;
    private string? _stravaClientSecret;
    private string? _stravaExpiresAt;
    private string? _stravaFirstname;
    private string? _stravaLastname;
    private string? _stravaRefreshToken;
    private string? _stravaUsername;

    public string OpenWeatherAppId
    {
        get => _openWeatherAppId = string.IsNullOrEmpty(_openWeatherAppId) ? string.Empty : _openWeatherAppId;
        set => _openWeatherAppId = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaAthleteId
    {
        get => _stravaAthleteId = string.IsNullOrEmpty(_stravaAthleteId) ? string.Empty : _stravaAthleteId;
        set => _stravaAthleteId = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaClientId
    {
        get => _stravaClientId = string.IsNullOrEmpty(_stravaClientId) ? string.Empty : _stravaClientId;
        set => _stravaClientId = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaClientSecret
    {
        get => _stravaClientSecret = string.IsNullOrEmpty(_stravaClientSecret) ? string.Empty : _stravaClientSecret;
        set => _stravaClientSecret = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaUsername
    {
        get => _stravaUsername = string.IsNullOrEmpty(_stravaUsername) ? string.Empty : _stravaUsername;
        set => _stravaUsername = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaFirstname
    {
        get => _stravaFirstname = string.IsNullOrEmpty(_stravaFirstname) ? string.Empty : _stravaFirstname;
        set => _stravaFirstname = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaLastname
    {
        get => _stravaLastname = string.IsNullOrEmpty(_stravaLastname) ? string.Empty : _stravaLastname;
        set => _stravaLastname = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaRefreshToken
    {
        get => _stravaRefreshToken = string.IsNullOrEmpty(_stravaRefreshToken) ? string.Empty : _stravaRefreshToken;
        set => _stravaRefreshToken = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaAccessToken
    {
        get => _stravaAccessToken = string.IsNullOrEmpty(_stravaAccessToken) ? string.Empty : _stravaAccessToken;
        set => _stravaAccessToken = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaExpiresAt
    {
        get => _stravaExpiresAt = string.IsNullOrEmpty(_stravaExpiresAt) ? string.Empty : _stravaExpiresAt;
        set => _stravaExpiresAt = string.IsNullOrEmpty(value) ? string.Empty : value;
    }

    public string StravaExpiresAtFormatted()
    {
        var b = long.TryParse(StravaExpiresAt, out var seconds);
        if (!b)
        {
            return string.Empty;
        }

        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
        return $"{dateTime:dddd, dd MMMM yyyy HH:mm} ({StravaExpiresAt})";
    }

    public override string ToString()
    {
        return
            $"{nameof(OpenWeatherAppId)}: {OpenWeatherAppId}, {nameof(StravaAthleteId)}: {StravaAthleteId}, {nameof(StravaClientId)}: {StravaClientId}, {nameof(StravaClientSecret)}: {StravaClientSecret}, {nameof(StravaUsername)}: {StravaUsername}, {nameof(StravaFirstname)}: {StravaFirstname}, {nameof(StravaLastname)}: {StravaLastname}, {nameof(StravaRefreshToken)}: {StravaRefreshToken}, {nameof(StravaAccessToken)}: {StravaAccessToken}, {nameof(StravaExpiresAt)}: {StravaExpiresAt}";
    }
}
namespace Uroskur.Services;

public class PreferencesService : IPreferencesService
{
    public string FindPreference(string key)
    {
        return Microsoft.Maui.Storage.Preferences.Get(key, string.Empty);
    }

    public Preferences FindPreferences()
    {
        return new Preferences
        {
            OpenWeatherAppId = FindPreference(OpenWeatherAppIdKey),
            StravaAccessToken = FindPreference(StravaAccessTokenKey),
            StravaAthleteId = FindPreference(StravaAthleteIdKey),
            StravaClientId = FindPreference(StravaClientIdKey),
            StravaClientSecret = FindPreference(StravaClientSecretKey),
            StravaExpiresAt = FindPreference(StravaExpiresAtKey),
            StravaFirstname = FindPreference(StravFirstNameKey),
            StravaLastname = FindPreference(StravaLastNameKey),
            StravaRefreshToken = FindPreference(StravaRefreshTokenKey),
            StravaUsername = FindPreference(StravaUsernameKey)
        };
    }

    public void SavePreference(string key, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        Microsoft.Maui.Storage.Preferences.Set(key, value.Trim());
    }

    public void SavePreferences(Preferences preferences)
    {
        SavePreference(OpenWeatherAppIdKey, preferences.OpenWeatherAppId);
        SavePreference(StravaAccessTokenKey, preferences.StravaAccessToken);
        SavePreference(StravaAthleteIdKey, preferences.StravaAthleteId);
        SavePreference(StravaClientIdKey, preferences.StravaClientId);
        SavePreference(StravaClientSecretKey, preferences.StravaClientSecret);
        SavePreference(StravaExpiresAtKey, preferences.StravaExpiresAt);
        SavePreference(StravFirstNameKey, preferences.StravaFirstname);
        SavePreference(StravaLastNameKey, preferences.StravaLastname);
        SavePreference(StravaRefreshTokenKey, preferences.StravaRefreshToken);
        SavePreference(StravaUsernameKey, preferences.StravaUsername);
    }
}
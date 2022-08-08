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
            OpenWeatherAppId = FindPreference("openweatherAppId"),
            StravaAccessToken = FindPreference("stravaAccessToken"),
            StravaAthleteId = FindPreference("stravaAthleteId"),
            StravaClientId = FindPreference("stravaClientId"),
            StravaClientSecret = FindPreference("stravaClientSecret"),
            StravaExpiresAt = FindPreference("stravaExpiresAt"),
            StravaFirstname = FindPreference("stravaFirstname"),
            StravaLastname = FindPreference("stravaLastname"),
            StravaRefreshToken = FindPreference("stravaRefreshToken"),
            StravaUsername = FindPreference("stravaUsername")
        };
    }

    public void SavePreference(string key, string value)
    {
        Microsoft.Maui.Storage.Preferences.Set(key, value);
    }

    public void SavePreferences(Preferences preferences)
    {
        SavePreference("openweatherAppId", preferences.OpenWeatherAppId!);
        SavePreference("stravaAccessToken", preferences.StravaAccessToken!);
        SavePreference("stravaAthleteId", preferences.StravaAthleteId!);
        SavePreference("stravaClientId", preferences.StravaClientId!);
        SavePreference("stravaClientSecret", preferences.StravaClientSecret!);
        SavePreference("stravaExpiresAt", preferences.StravaExpiresAt!);
        SavePreference("stravaFirstname", preferences.StravaFirstname!);
        SavePreference("stravaLastname", preferences.StravaLastname!);
        SavePreference("stravaRefreshToken", preferences.StravaRefreshToken!);
        SavePreference("stravaUsername", preferences.StravaUsername!);
    }
}
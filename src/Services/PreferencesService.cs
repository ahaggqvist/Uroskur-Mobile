namespace Uroskur.Services;

public class PreferencesService : IPreferencesService
{
    public string FindPreference(string key)
    {
        return Preferences.Get(key, string.Empty);
    }

    public AppPreferences FindPreferences()
    {
        return new AppPreferences
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
        Preferences.Set(key, value);
    }

    public void SavePreferences(AppPreferences appPreferences)
    {
        SavePreference("openweatherAppId", appPreferences.OpenWeatherAppId!);
        SavePreference("stravaAccessToken", appPreferences.StravaAccessToken!);
        SavePreference("stravaAthleteId", appPreferences.StravaAthleteId!);
        SavePreference("stravaClientId", appPreferences.StravaClientId!);
        SavePreference("stravaClientSecret", appPreferences.StravaClientSecret!);
        SavePreference("stravaExpiresAt", appPreferences.StravaExpiresAt!);
        SavePreference("stravaFirstname", appPreferences.StravaFirstname!);
        SavePreference("stravaLastname", appPreferences.StravaLastname!);
        SavePreference("stravaRefreshToken", appPreferences.StravaRefreshToken!);
        SavePreference("stravaUsername", appPreferences.StravaUsername!);
    }
}
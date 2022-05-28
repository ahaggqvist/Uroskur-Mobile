namespace Uroskur.Services;

public interface IPreferencesService
{
    string FindPreference(string key);

    AppPreferences FindPreferences();

    void SavePreference(string key, string value);

    void SavePreferences(AppPreferences appPreferences);
}
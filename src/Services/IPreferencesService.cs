namespace Uroskur.Services;

public interface IPreferencesService
{
    string FindPreference(string key);

    Preferences FindPreferences();

    void SavePreference(string key, string value);

    void SavePreferences(Preferences preferences);
}
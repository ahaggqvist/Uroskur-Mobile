using CommunityToolkit.Mvvm.Input;

namespace Uroskur.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaService _stravaService;
    [ObservableProperty] private string? _openWeatherAppId;
    [ObservableProperty] private string? _stravaAccessToken;
    [ObservableProperty] private string? _stravaAthleteId;
    [ObservableProperty] private string? _stravaClientId;
    [ObservableProperty] private string? _stravaClientSecret;
    [ObservableProperty] private string? _stravaExpiresAt;
    [ObservableProperty] private string? _stravaExpiresAtFormatted;
    [ObservableProperty] private string? _stravaFirstname;
    [ObservableProperty] private string? _stravaLastname;
    [ObservableProperty] private string? _stravaRefreshToken;
    [ObservableProperty] private string? _stravaUsername;

    public SettingsViewModel(IPreferencesService preferencesService, IStravaService stravaService)
    {
        Title = "Settings";

        _preferencesService = preferencesService;
        _stravaService = stravaService;

        LoadPreferences();
    }


    [RelayCommand]
    private void SavePreferences()
    {
        _preferencesService.SavePreferences(new AppPreferences
        {
            OpenWeatherAppId = _openWeatherAppId!,
            StravaClientId = _stravaClientId!,
            StravaClientSecret = _stravaClientSecret!
        });
    }

    [RelayCommand]
    private async void TokenExchange()
    {
        ResetStravaPreferences();

        var isSuccessfulExchange = await _stravaService.TokenExchangeAsync();
        Debug.WriteLine($"Token exchange {isSuccessfulExchange}");

        LoadPreferences();
    }

    public void LoadPreferences()
    {
        var preferences = _preferencesService.FindPreferences();
        _openWeatherAppId = preferences.OpenWeatherAppId;
        _stravaClientId = preferences.StravaClientId;
        _stravaAthleteId = preferences.StravaAthleteId;
        _stravaClientSecret = preferences.StravaClientSecret;
        _stravaUsername = preferences.StravaUsername;
        _stravaFirstname = preferences.StravaFirstname;
        _stravaLastname = preferences.StravaLastname;
        _stravaRefreshToken = preferences.StravaRefreshToken;
        _stravaAccessToken = preferences.StravaAccessToken;
        _stravaExpiresAt = preferences.StravaExpiresAt;
        _stravaExpiresAtFormatted = preferences.StravaExpiresAtFormatted();

        OnPropertyChanged(nameof(OpenWeatherAppId));
        OnPropertyChanged(nameof(StravaClientId));
        OnPropertyChanged(nameof(StravaAthleteId));
        OnPropertyChanged(nameof(StravaClientSecret));
        OnPropertyChanged(nameof(StravaUsername));
        OnPropertyChanged(nameof(StravaFirstname));
        OnPropertyChanged(nameof(StravaLastname));
        OnPropertyChanged(nameof(StravaRefreshToken));
        OnPropertyChanged(nameof(StravaAccessToken));
        OnPropertyChanged(nameof(StravaExpiresAt));
        OnPropertyChanged(nameof(StravaExpiresAtFormatted));
    }

    private void ResetStravaPreferences()
    {
        var preferences = _preferencesService.FindPreferences();
        StravaAthleteId = string.Empty;
        StravaUsername = string.Empty;
        StravaFirstname = string.Empty;
        StravaLastname = string.Empty;
        StravaRefreshToken = string.Empty;
        StravaAccessToken = string.Empty;
        StravaExpiresAt = string.Empty;
        _preferencesService.SavePreferences(preferences);

        OnPropertyChanged(nameof(StravaAthleteId));
        OnPropertyChanged(nameof(StravaUsername));
        OnPropertyChanged(nameof(StravaFirstname));
        OnPropertyChanged(nameof(StravaLastname));
        OnPropertyChanged(nameof(StravaRefreshToken));
        OnPropertyChanged(nameof(StravaAccessToken));
        OnPropertyChanged(nameof(StravaExpiresAt));
        OnPropertyChanged(nameof(StravaExpiresAtFormatted));
    }
}
namespace Uroskur.ViewModels;

public partial class PreferencesViewModel : BaseViewModel
{
    private const string PatternClientId = @"^(\d{4,})$";
    private const string PatternKey = @"^([\w\d]{15,})$";
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaService _stravaService;
    [ObservableProperty] private string _errorMessage = string.Empty;
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

    public PreferencesViewModel(IPreferencesService preferencesService, IStravaService stravaService)
    {
        _preferencesService = preferencesService;
        _stravaService = stravaService;
    }

    [RelayCommand]
    private async void SavePreferences()
    {
        if (!Regex.IsMatch(StravaClientId ?? string.Empty, PatternClientId))
        {
            await ShowToast("Strava client ID is invalid");
            return;
        }

        if (!Regex.IsMatch(StravaClientSecret ?? string.Empty, PatternKey))
        {
            await ShowToast("Strava client secret is invalid");
            return;
        }

        if (!Regex.IsMatch(OpenWeatherAppId ?? string.Empty, PatternKey))
        {
            await ShowToast("OpenWeather app ID is invalid");
            return;
        }

        _preferencesService.SavePreferences(new Preferences
        {
            OpenWeatherAppId = OpenWeatherAppId!,
            StravaClientId = StravaClientId!,
            StravaClientSecret = StravaClientSecret!
        });

        await ShowToast("Saved settings");
    }

    [RelayCommand]
    private async void ConnectWithStrava()
    {
        if (!Regex.IsMatch(_preferencesService.FindPreference(StravaClientIdKey) ?? string.Empty, PatternClientId))
        {
            await ShowToast("Strava client ID is invalid");
            return;
        }

        if (!Regex.IsMatch(_preferencesService.FindPreference(StravaClientSecretKey) ?? string.Empty, PatternKey))
        {
            await ShowToast("Strava client secret is invalid");
            return;
        }

        if (!Regex.IsMatch(_preferencesService.FindPreference(OpenWeatherAppIdKey) ?? string.Empty, PatternKey))
        {
            await ShowToast("OpenWeather app ID is invalid");
            return;
        }

        if (!await _stravaService.TokenExchangeAsync())
        {
            await ShowToast("Strava authorization failed");
            return;
        }

        await ShowToast("Strava authorization was successful");

        LoadPreferences();
    }

    public void LoadPreferences()
    {
        var preferences = _preferencesService.FindPreferences();
        OpenWeatherAppId = preferences.OpenWeatherAppId;
        StravaClientId = preferences.StravaClientId;
        StravaAthleteId = preferences.StravaAthleteId;
        StravaClientSecret = preferences.StravaClientSecret;
        StravaUsername = preferences.StravaUsername;
        StravaFirstname = preferences.StravaFirstname;
        StravaLastname = preferences.StravaLastname;
        StravaRefreshToken = preferences.StravaRefreshToken;
        StravaAccessToken = preferences.StravaAccessToken;
        StravaExpiresAt = preferences.StravaExpiresAt;
        StravaExpiresAtFormatted = preferences.StravaExpiresAtFormatted();
    }

    partial void OnStravaClientIdChanged(string? value)
    {
        if (Regex.IsMatch(value ?? string.Empty, PatternClientId))
        {
            _preferencesService.SavePreference(StravaClientIdKey, value!);
        }
    }

    partial void OnStravaClientSecretChanged(string? value)
    {
        if (Regex.IsMatch(value ?? string.Empty, PatternKey))
        {
            _preferencesService.SavePreference(StravaClientSecretKey, value!);
        }
    }

    partial void OnOpenWeatherAppIdChanged(string? value)
    {
        if (Regex.IsMatch(value ?? string.Empty, PatternKey))
        {
            _preferencesService.SavePreference(OpenWeatherAppIdKey, value!);
        }
    }

    private static async Task ShowToast(string text, ToastDuration duration = ToastDuration.Short, double fontSize = 13)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make(text, duration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }
}
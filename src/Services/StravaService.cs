﻿namespace Uroskur.Services;

public class StravaService : IStravaService
{
    private const int MaxLocations = 100;
#if DEBUG
    private const int ExpireInHours = 24;
#else
    private const int ExpireInHours = 1;
#endif
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaClient _stravaClient;

    public StravaService(IStravaClient stravaClient, IPreferencesService preferencesService)
    {
        _stravaClient = stravaClient;
        _preferencesService = preferencesService;
    }

    public async Task<bool> TokenExchangeAsync()
    {
        var preferences = _preferencesService.FindPreferences();
        var clientId = preferences.StravaClientId;
        var clientSecret = preferences.StravaClientSecret;

        var token = await _stravaClient.FetchAuthorizationTokenAsync(clientId, clientSecret);
        if (token?.Athlete == null)
        {
            Debug.WriteLine("Athlete is null.");
            return false;
        }

        Debug.WriteLine($"Token: {token}.");

        preferences.StravaAthleteId = token.Athlete.Id!.ToString()!;
        preferences.StravaUsername = token.Athlete.Username!;
        preferences.StravaFirstname = token.Athlete.Firstname!;
        preferences.StravaLastname = token.Athlete.Lastname!;
        preferences.StravaRefreshToken = token.RefreshToken!;
        preferences.StravaAccessToken = token.AccessToken!;
        preferences.StravaExpiresAt = token.ExpiresAt!.ToString()!;
        _preferencesService.SavePreferences(preferences);

        Debug.WriteLine($"Preferences {preferences}.");
        return true;
    }

    public async Task<IEnumerable<Routes>> FindRoutesByAthleteIdAsync(string? athleteId)
    {
        if (string.IsNullOrEmpty(athleteId))
        {
            Debug.WriteLine($"Athlete ID: {athleteId} is invalid.");

            return Array.Empty<Routes>();
        }

        var authorizationToken = await AuthorizationTokenByAthleteIdAsync(athleteId);
        return await _stravaClient.FetchRoutesAsync(athleteId, authorizationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Location>> FindLocationsByAthleteIdRouteIdAsync(string? athleteId, string? routeId)
    {
        if (string.IsNullOrEmpty(athleteId))
        {
            Debug.WriteLine($"Athlete ID: {athleteId} is invalid.");
            return Array.Empty<Location>();
        }

        if (string.IsNullOrEmpty(routeId))
        {
            Debug.WriteLine($"Route ID: {routeId} is invalid.");
            return Array.Empty<Location>();
        }

        var authorizationToken = await AuthorizationTokenByAthleteIdAsync(athleteId);

        Debug.WriteLine($"Is gxp cache expired: {Barrel.Current.IsExpired(routeId)}.");

        var gpx = Barrel.Current.Get<string>(routeId);
        try
        {
            if (string.IsNullOrEmpty(gpx) || Barrel.Current.IsExpired(routeId))
            {
                gpx = await _stravaClient.FetchGxpAsync(routeId, authorizationToken);

                Barrel.Current.Add(routeId, gpx, TimeSpan.FromHours(ExpireInHours));

                Debug.WriteLine($"Cache gxp route with Route ID: {routeId}.");
            }

            if (string.IsNullOrEmpty(gpx))
            {
                Debug.WriteLine("Gxp is blank.");
                return Array.Empty<Location>();
            }

            var gxpLocations = GpxParser.GpxToLocations(gpx);
            var locations = LocationHelper.FilterOutLocationsAtEvenDistances(gxpLocations);
            var locationsArray = locations.ToImmutableArray();
            if (locationsArray.Length <= MaxLocations)
            {
                return locationsArray;
            }

            Debug.WriteLine($"Number of locations: {locationsArray.Length} exceed the maximum allowed number of locations: {MaxLocations}.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Finding locations by Athlete ID and Route ID failed: {ex.Message} {ex.StackTrace}");
        }

        return Array.Empty<Location>();
    }

    private async Task<string?> AuthorizationTokenByAthleteIdAsync(string athleteId)
    {
        if (string.IsNullOrEmpty(athleteId))
        {
            Debug.WriteLine($"Athlete ID: {athleteId} is invalid.");
            return null;
        }

        var preferences = _preferencesService.FindPreferences();

        if (!IsTokenRefresh(long.Parse(preferences.StravaExpiresAt)))
        {
            return preferences.StravaAccessToken;
        }

        var authorizationToken = await RefreshTokenAsync(preferences.StravaRefreshToken);

        preferences.StravaAccessToken = authorizationToken?.AccessToken!;
        preferences.StravaExpiresAt = authorizationToken?.ExpiresAt.ToString()!;
        preferences.StravaRefreshToken = authorizationToken?.RefreshToken!;

        _preferencesService.SavePreferences(preferences);

        return authorizationToken?.AccessToken;
    }

    private async Task<AuthorizationToken?> RefreshTokenAsync(string refreshToken)
    {
        var preferences = _preferencesService.FindPreferences();
        var clientId = preferences.StravaClientId;
        var clientSecret = preferences.StravaClientSecret;

        return await _stravaClient.FetchRefreshTokenAsync(refreshToken, clientId, clientSecret).ConfigureAwait(false);
    }

    private static bool IsTokenRefresh(long expiresAt)
    {
        return DateTime.Now >= DateTime.UnixEpoch.AddSeconds(expiresAt).ToLocalTime();
    }
}
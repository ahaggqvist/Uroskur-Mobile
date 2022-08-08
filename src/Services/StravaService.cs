using Uroskur.Models.Strava;

namespace Uroskur.Services;

public class StravaService : IStravaService
{
    private const int HoursToExpire = 1;
    private const int MaxDistances = 100;
    private readonly IPreferencesService _preferencesService;
    private readonly IStravaClient _stravaClient;

    public StravaService(IStravaClient stravaClient, IPreferencesService preferencesService)
    {
        _stravaClient = stravaClient;
        _preferencesService = preferencesService;
    }

    public async Task<bool?> TokenExchangeAsync()
    {
        var preferences = _preferencesService.FindPreferences();
        var clientId = preferences.StravaClientId;
        var clientSecret = preferences.StravaClientSecret;

        var token = await _stravaClient.GetAuthorizationTokenAsync(clientId, clientSecret);
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

    public Task<string?> CreateSubscriptionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool?> DeleteSubscriptionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Subscription?> ViewSubscriptionAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Routes>> FindRoutesByAthleteIdAsync(string? athleteId)
    {
        if (string.IsNullOrEmpty(athleteId))
        {
            Debug.WriteLine($"Athlete ID: {athleteId} is invalid.");

            return Array.Empty<Routes>();
        }

        var authorizationToken = await AuthorizationTokenByAthleteIdAsync(athleteId);
        return await _stravaClient.GetRoutesAsync(athleteId, authorizationToken).ConfigureAwait(false);
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
                gpx = await _stravaClient.GetGxpAsync(routeId, authorizationToken);

                Barrel.Current.Add(routeId, gpx, TimeSpan.FromHours(HoursToExpire));

                Debug.WriteLine($"Cache gxp route with route ID: {routeId}.");
            }

            if (string.IsNullOrEmpty(gpx))
            {
                Debug.WriteLine("Gxp is blank.");
                return Array.Empty<Location>();
            }

            var parsedLocations = GpxParser.GpxToLocations(gpx);

            Debug.WriteLine($"Total locations: {parsedLocations.Count}");

            var distances = DistanceHelper.GetEvenDistances(parsedLocations);
            var locations = distances.ToImmutableArray();
            if (locations.Length <= MaxDistances)
            {
                return locations;
            }

            Debug.WriteLine($"Even distances: {locations.Length} exceed maximum: {MaxDistances}.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Find locations by athleteId routeId async failed: {ex.Message} {ex.StackTrace}");
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

        return await _stravaClient.GetRefreshTokenAsync(refreshToken, clientId, clientSecret).ConfigureAwait(false);
    }

    private static bool IsTokenRefresh(long expiresAt)
    {
        return DateTime.Now >= DateTime.UnixEpoch.AddSeconds(expiresAt).ToLocalTime();
    }
}
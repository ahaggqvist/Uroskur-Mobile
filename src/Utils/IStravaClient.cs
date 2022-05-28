namespace Uroskur.Utils;

public interface IStravaClient
{
    Task<AuthorizationToken?> GetAuthorizationTokenAsync(string? clientId, string? clientSecret);

    Task<string?> CreateSubscriptionAsync(string? clientId, string? clientSecret);

    Task<bool?> DeleteSubscriptionAsync(string? clientId, string? clientSecret);

    Task<Subscription?> ViewSubscriptionAsync(string? clientId, string? clientSecret);

    Task<AuthorizationToken?> GetRefreshTokenAsync(string? refreshToken, string? clientId,
        string? clientSecret);

    Task<IEnumerable<Routes>> GetRoutesAsync(string? athleteId, string? authorizationToken);

    Task<string> GetGxpAsync(string? routeId, string? authorizationToken);
}
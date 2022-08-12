namespace Uroskur.Utils.Clients;

public interface IStravaClient
{
    Task<AuthorizationToken?> FetchAuthorizationTokenAsync(string? clientId, string? clientSecret);

    Task<string?> CreateSubscriptionAsync(string? clientId, string? clientSecret);

    Task<bool?> DeleteSubscriptionAsync(string? clientId, string? clientSecret);

    Task<Subscription?> ViewSubscriptionAsync(string? clientId, string? clientSecret);

    Task<AuthorizationToken?> FetchRefreshTokenAsync(string? refreshToken, string? clientId,
        string? clientSecret);

    Task<IEnumerable<Routes>> FetchRoutesAsync(string? athleteId, string? authorizationToken);

    Task<string> FetchGxpAsync(string? routeId, string? authorizationToken);
}
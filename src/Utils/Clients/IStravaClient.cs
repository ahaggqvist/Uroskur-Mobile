namespace Uroskur.Utils.Clients;

public interface IStravaClient
{
    Task<AuthorizationToken?> FetchAuthorizationTokenAsync(string? clientId, string? clientSecret);

    Task<AuthorizationToken?> FetchRefreshTokenAsync(string? refreshToken, string? clientId,
        string? clientSecret);

    Task<IEnumerable<Routes>> FetchRoutesAsync(string? athleteId, string? authorizationToken);

    Task<string> FetchGxpAsync(string? routeId, string? authorizationToken);
}
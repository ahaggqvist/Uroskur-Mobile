namespace Uroskur.Services;

public interface IStravaService
{
    Task<bool?> TokenExchangeAsync();

    Task<string?> CreateSubscriptionAsync();

    Task<bool?> DeleteSubscriptionAsync();

    Task<Subscription?> ViewSubscriptionAsync();

    Task<IEnumerable<Routes>> FindRoutesByAthleteIdAsync(string? athleteId);

    Task<IEnumerable<Location>> FindLocationsByAthleteIdRouteIdAsync(string? athleteId, string? routeId);
}
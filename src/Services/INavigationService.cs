namespace Uroskur.Services;

public interface INavigationService
{
    Task GoBackAsync();
    Task GoBackModalAsync();
    Task NavigateToAsync(string route);
    Task NavigateToAsync(string route, Dictionary<string, object> parameters, bool animate = true);
}
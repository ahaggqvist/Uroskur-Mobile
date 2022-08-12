namespace Uroskur.Services;

public class RoutingService : IRoutingService
{
    public Task NavigateToAsync(string route)
    {
        return Shell.Current.GoToAsync(route);
    }

    public Task NavigateToAsync(string route, Dictionary<string, object> parameters, bool animate = true)
    {
        if (parameters?.Count == 0)
        {
            parameters = new Dictionary<string, object>();
        }

        return Shell.Current.GoToAsync(route, animate, parameters);
    }

    public Task GoBackAsync()
    {
        return Shell.Current.Navigation.PopAsync();
    }

    public Task GoBackModalAsync()
    {
        return Shell.Current.Navigation.PopModalAsync();
    }
}
namespace Uroskur.Services;

public class RoutingService : IRoutingService
{
    public Task NavigateToAsync(string route)
    {
        Debug.WriteLine($"The URI of the current page: {Shell.Current.CurrentState.Location}.");

        return Shell.Current.GoToAsync(route);
    }

    public Task NavigateToAsync(string route, Dictionary<string, object> parameters, bool animate = true)
    {
        if (parameters?.Count == 0)
        {
            parameters = new Dictionary<string, object>();
        }

        Debug.WriteLine($"The URI of the current page: {Shell.Current.CurrentState.Location}.");

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
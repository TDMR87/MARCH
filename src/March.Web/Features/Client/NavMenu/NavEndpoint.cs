namespace March.Web.Features.Client.NavMenu;

public class NavEndpoint
{
    public record Request(string IsCollapsed);

    public static IResult GetNav()
    {
        return Component<Nav>();
    }

    public static IResult ToggleNav(Request request)
    {
        return Component<Nav, NavModel>(model: new()
        {
            IsCollapsed = !bool.Parse(request.IsCollapsed)
        });
    }
}

namespace March.Web.Features.Client.NavMenu;

public class NavEndpoint
{
    public record Request(bool IsCollapsed);

    public static IResult GetNav()
    {
        return Component<NavItems>();
    }

    public static IResult ToggleNav(Request request)
    {
        return Component<NavItems, NavModel>(model: new()
        {
            IsCollapsed = request.IsCollapsed
        });
    }
}

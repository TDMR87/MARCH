namespace March.Web.Features.Client.NavMenu;

public class NavEndpoint
{
    public record Request(bool IsCollapsed);

    public static IResult GetNavItems()
    {
        return Component<NavItems>();
    }
}

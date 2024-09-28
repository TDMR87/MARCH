namespace March.Web.Features.Client.NavMenu;

public class NavEndpoint
{
    public record Request(string IsCollapsed);

    public static IResult ToggleNav(Request request, CancellationToken cancellationToken)
    {
        return Component<Nav, NavModel>(model: new()
        {
            IsCollapsed = !bool.Parse(request.IsCollapsed)
        });
    }
}

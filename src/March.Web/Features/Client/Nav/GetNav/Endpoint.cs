namespace March.Web.Features.Client.Nav.GetNav;

public class Endpoint
{
    public const string Path = "/nav-items";
    private const string summary = "Get the navigation menu";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.NavItems>>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.NavItems>();
    }
}

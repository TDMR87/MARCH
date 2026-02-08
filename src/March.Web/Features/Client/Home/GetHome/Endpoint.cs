namespace March.Web.Features.Client.Home.GetHome;

public class Endpoint
{
    public const string Path = "/home";
    private const string summary = "Get the home page content";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.Home>>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.Home>();
    }
}

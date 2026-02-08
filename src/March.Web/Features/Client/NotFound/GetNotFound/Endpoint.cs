namespace March.Web.Features.Client.NotFound.GetNotFound;

public class Endpoint
{
    public const string Path = "/notfound";
    private const string summary = "Not found page";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.NotFound>>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.NotFound>();
    }
}

namespace March.Web.Features.Client.Counter.GetCounter;

public class Endpoint
{
    public const string Path = "/counter";
    private const string summary = "Get a counter component";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.Counter>>()
            .WithFeatureFlags(FeatureFlag.Counter)
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.Counter>();
    }
}

namespace March.Web.Features.Client.HowItWorks.GetHowItWorks;

public class Endpoint
{
    public const string Path = "/how-it-works";
    private const string summary = "Get a how-it-works page";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.HowItWorks>>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.HowItWorks>();
    }
}

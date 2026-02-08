namespace March.Web.Features.Client.Form.GetForm;

public class Endpoint
{
    public const string Path = "/form";
    private const string summary = "Get an input form";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.Form>>()
            .WithFeatureFlags(FeatureFlag.Form)
            .AllowAnonymous()
            .WithSummary(summary);
    }

    private static IResult Handler()
    {
        return new RazorComponentResult<Components.Form>();
    }
}

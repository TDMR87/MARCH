namespace March.Web.Features.Client.Counter.UpdateCounter;

public class Endpoint
{
    public const string Path = "/counter/update";
    private const string summary = "Update a counter component";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RazorComponentResult<Components.Counter>>()
            .WithFeatureFlags(FeatureFlag.Counter)
            .WithValidation<Validator>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler(Request request)
    {
        return new RazorComponentResult<Components.Counter>(new
        {
            Count = request.Count
        });
    }
}

namespace March.Web.Features.Client.Admin.ToggleFeatureFlag;

public class Endpoint
{
    public const string Path = "/admin/feature-toggle";
    private const string summary = "Toggles a feature on or off";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RazorComponentResult<Components.AdminDashboard>>()
            .RequireAuthorization()
            .WithSummary(summary);
    }

    public static IResult Handler(Request request)
    {
        return new RazorComponentResult<Components.AdminDashboard>();
    }
}

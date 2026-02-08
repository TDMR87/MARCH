namespace March.Web.Features.Client.Admin.GetDashboard;

public class Endpoint
{
    public const string Path = "/admin/dashboard";
    private const string summary = "Get the admin dashboard";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.AdminDashboard>>()
            .RequireAuthorization()
            .WithSummary(summary);
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.AdminDashboard>();
    }
}

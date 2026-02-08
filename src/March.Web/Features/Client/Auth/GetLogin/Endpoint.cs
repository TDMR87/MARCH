namespace March.Web.Features.Client.Auth.GetLogin;

public class Endpoint
{
    public const string Path = "/login";
    private const string summary = "Get a login form";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, () =>
            {
                return new RazorComponentResult<Components.Login>();
            })
            .Produces<RazorComponentResult<Components.Login>>()
            .AllowAnonymous()
            .WithSummary(summary);
    }
}

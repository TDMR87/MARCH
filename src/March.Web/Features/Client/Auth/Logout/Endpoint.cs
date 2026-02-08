namespace March.Web.Features.Client.Auth.Logout;

public class Endpoint
{
    public const string Path = "/logout";
    private const string summary = "Log out";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RazorComponentResult<Home.Components.Home>>()
            .RequireAuthorization()
            .WithSummary(summary);
    }

    public static async Task<IResult> Handler(HttpContextAccessor httpContextAccessor)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(Constants.MarchAuthCookie);
        await httpContextAccessor.HttpContext!.SignOutAsync(Constants.MarchAuthCookie);
        httpContextAccessor.HttpContext?.Response.Headers["HX-Trigger"] = "logout";
        return new RazorComponentResult<Home.Components.Home>();
    }
}

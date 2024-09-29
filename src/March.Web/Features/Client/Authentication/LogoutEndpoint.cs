namespace March.Web.Features.Client.Authentication;

public class LogoutEndpoint
{
    public static async Task<IResult> Logout(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(Constants.MarchAuthCookie);
        httpContext.Response.Headers["HX-Trigger"] = "logout";
        return Component<Home.Home>();
    }
}

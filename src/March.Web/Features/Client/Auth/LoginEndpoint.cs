
namespace March.Web.Features.Client.Authentication;

public class LoginEndpoint
{
    public record LoginRequest(string Username, string Password, string? ReturnUrl = null);

    public static IResult GetLogin()
    {
        return Component<Login>();
    }

    public static async Task<IResult> SubmitLogin(
        LoginRequest request, 
        ValidationContext validationContext, 
        HttpContext httpContext)
    {
        if (validationContext.Errors.Any())
        {
            return Component<Login, LoginFormModel>(model: new()
            {
                Username = request.Username,
                Password = request.Password
            });
        }

        await httpContext.SignInAsync(Constants.MarchAuthCookie, 
            new ClaimsPrincipal(
                new ClaimsIdentity(
                [
                    new(ClaimTypes.Name, request.Username)
                ], 
                Constants.MarchAuthCookie)));

        // Triggers a login event in the client DOM
        httpContext.Response.Headers["HX-Trigger"] = "login";

        if (string.IsNullOrWhiteSpace(request.ReturnUrl) || request.ReturnUrl == "/")
        {
            return Component<Home.Home>();
        }
        else
        {
            return Results.Redirect(request.ReturnUrl);
        }
    }
}

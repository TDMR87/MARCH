using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace March.Web.Features.Client.Authentication;

public class LoginEndpoint
{
    public record LoginRequest(string Username, string Password);

    public static IResult GetLogin()
    {
        return Component<Login>();
    }

    public static async Task<IResult> SubmitLogin(LoginRequest request, ValidationContext validationContext, HttpContext httpContext)
    {
        if (validationContext.Errors.Any())
        {
            return Component<Login, LoginFormModel>(model: new()
            {
                Username = request.Username,
                Password = request.Password
            });
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, request.Username)
        };

        await httpContext.SignInAsync("MarchCookieAuth", new ClaimsPrincipal(new ClaimsIdentity(
        [
            new(ClaimTypes.Name, request.Username)
        ], "MarchCookieAuth")));

        httpContext.Response.Headers["HX-Trigger"] = "login";

        return Component<Home.Home>();
    }
}

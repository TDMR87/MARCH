namespace March.Web.Features.Client.Auth.SubmitLogin;

public class Endpoint
{
    public const string Path = "/login";
    private const string summary = "Submit a login request";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RedirectHttpResult>()
            .Produces<RazorComponentResult<Home.Components.Home>>()
            .WithValidation<Validator>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static async Task<IResult> Handler(
        Request request,
        ValidationContext validationContext,
        HttpContext httpContext)
    {
        if (validationContext.Errors.Any())
        {
            return new RazorComponentResult<Components.Login>(new
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

        httpContext.Response.Headers["HX-Trigger"] = "login";

        if (string.IsNullOrWhiteSpace(request.ReturnUrl) || request.ReturnUrl == "/")
        {
            return new RazorComponentResult<Home.Components.Home>();
        }

        return Results.Redirect(request.ReturnUrl);
    }
}

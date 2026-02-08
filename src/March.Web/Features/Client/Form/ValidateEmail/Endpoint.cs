namespace March.Web.Features.Client.Form.ValidateEmail;

public class Endpoint
{
    public const string Path = "/form/email";
    private const string summary = "Validate email";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RazorComponentResult<Components.FormEmail>>()
            .WithValidation<Validator>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler(Request request) =>
        new RazorComponentResult<Components.FormEmail>(new
        {
            Email = request.Email
        });
}

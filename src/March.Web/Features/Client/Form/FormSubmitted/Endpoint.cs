namespace March.Web.Features.Client.Form.FormSubmitted;

public class Endpoint
{
    public const string Path = "/form/submitted";
    private const string summary = "Toggle a form submitted modal";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RazorComponentResult<Components.FormSubmitted>>()
            .WithFeatureFlags(FeatureFlag.FormSubmit)
            .AllowAnonymous()
            .WithSummary(summary);
    }

    private static IResult Handler(Request request)
    {
        if (request.Toggle == "false")
        {
            return new RazorComponentResult<Components.Form>();
        }

        return new RazorComponentResult<Components.FormSubmitted>(new
        {
            Toggle = true
        });
    }
}

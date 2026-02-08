namespace March.Web.Features.Client.Form.SubmitForm;

public class Endpoint
{
    public const string Path = "/form/submit";
    private const string summary = "Submit an input form";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RazorComponentResult<Components.FormSubmitted>>()
            .WithFeatureFlags(FeatureFlag.FormSubmit)
            .WithValidation<Validator>()
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler(Request request, ValidationContext validationContext)
    {
        if (!validationContext.Errors.Any())
        {
            return new RazorComponentResult<Components.FormSubmitted>();
        }

        return new RazorComponentResult<Components.Form>(new
        {
            Name = request.Name,
            Email = request.Email,
        });
    }
}

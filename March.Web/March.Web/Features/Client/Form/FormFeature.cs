namespace March.Web.Features.Client.Form;

public class FormFeature
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("form", HandleRequest)
        .AllowAnonymous()
        .WithSummary("Input form");

    public record Request();

    public class RequestValidator
    {
        public RequestValidator()
        {
            // TODO: add FluentValidation   
        }
    }

    private static RazorComponentResult HandleRequest(CancellationToken cancellationToken)
    {
        return Component<Form, FormModel>(model: new());
    }
}

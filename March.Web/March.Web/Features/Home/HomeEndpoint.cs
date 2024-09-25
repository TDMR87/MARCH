namespace March.Web.Features.Home;

public class HomeEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("home", HandleRequest)
        .AllowAnonymous()
        .WithSummary("Home page");

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
        return Component<Home>();
    }
}

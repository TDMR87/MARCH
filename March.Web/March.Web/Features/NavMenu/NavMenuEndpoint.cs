namespace March.Web.Features.NavMenu;

public class NavMenuEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("nav", HandleRequest)
        .AllowAnonymous()
        .WithSummary("Open and close nav menu");

    public record Request(string IsClosed);

    public class RequestValidator
    {
        public RequestValidator()
        {
            // TODO: add FluentValidation   
        }
    }

    private static RazorComponentResult HandleRequest(Request request, CancellationToken cancellationToken)
    {
        return Component<NavMenu, NavMenuModel>(model: new()
        {
            IsClosed = !bool.Parse(request.IsClosed)
        });
    }
}

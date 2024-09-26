namespace March.Web.Features.Client.NavMenu;

public class NavMenuFeature : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("nav", HandleRequest)
        .AllowAnonymous()
        .WithSummary("Open and close nav menu");

    public record Request(string IsCollapsed);

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
            IsCollapsed = !bool.Parse(request.IsCollapsed)
        });
    }
}

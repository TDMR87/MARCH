namespace March.Web.Features.Client.NavMenu;

public class NavMenuFeature
{
    public record Request(string IsCollapsed);

    public static RazorComponentResult HandleRequest(Request request, CancellationToken cancellationToken)
    {
        return Component<NavMenu, NavMenuModel>(model: new()
        {
            IsCollapsed = !bool.Parse(request.IsCollapsed)
        });
    }

    public class RequestValidator
    {
        public RequestValidator()
        {
            // TODO: add FluentValidation   
        }
    }
}

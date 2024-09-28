namespace March.Web.Features.Client.Home;

public class HomeFeature
{
    public record Request();

    public static RazorComponentResult HandleRequest(CancellationToken cancellationToken)
    {
        return Component<Home>();
    }

    public class RequestValidator
    {
        public RequestValidator()
        {
            // TODO: add FluentValidation   
        }
    }
}

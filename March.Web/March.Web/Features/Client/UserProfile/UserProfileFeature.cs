namespace March.Web.Features.Client.UserProfile;

public class UserProfileFeature
{
    public record Request();

    public static RazorComponentResult HandleRequest(CancellationToken cancellationToken)
    {
        return Component<UserProfile>();
    }

    public class RequestValidator
    {
        public RequestValidator()
        {
            // TODO: add FluentValidation   
        }
    }
}

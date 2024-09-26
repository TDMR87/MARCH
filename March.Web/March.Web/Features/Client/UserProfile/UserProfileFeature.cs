namespace March.Web.Features.Client.UserProfile;

public class UserProfileFeature : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("profile", HandleRequest)
        .AllowAnonymous()
        .WithSummary("User profile");

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
        return Component<UserProfile>();
    }
}

namespace March.Web.Features.Client.Home;

public class HomeEndpoint
{
    public record Request();

    public static IResult GetHome()
    {
        return Component<Home>();
    }
}

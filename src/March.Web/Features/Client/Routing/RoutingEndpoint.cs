namespace March.Web.Features.Client.Routing;

public class RoutingEndpoint
{
    public static IResult NotFound()
    {
        return Component<NotFound>();
    }
}

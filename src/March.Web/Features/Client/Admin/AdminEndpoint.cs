namespace March.Web.Features.Client.Admin;

public class AdminEndpoint
{
    public record Request();

    public static IResult GetDashboard(HttpRequest req)
    {
        return Component<AdminDashboard>();
    }
}

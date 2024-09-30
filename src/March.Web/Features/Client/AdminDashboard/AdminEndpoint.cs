namespace March.Web.Features.Client.AdminDashboard;

public class AdminEndpoint
{
    public record Request();

    public static IResult GetDashboard(HttpRequest req)
    {
        return Component<AdminDashboard>();
    }
}

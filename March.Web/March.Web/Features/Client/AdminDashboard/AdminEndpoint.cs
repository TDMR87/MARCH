namespace March.Web.Features.Client.AdminDashboard;

public class AdminEndpoint
{
    public record Request();

    public static IResult GetDashboard()
    {
        return Component<AdminDashboard>();
    }
}

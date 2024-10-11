namespace March.Web.Features.Client.Admin;

public class AdminEndpoint
{
    public record ToggleFeatureFlagRequest(bool Enabled);

    public static IResult GetDashboard()
    {
        return Component<AdminDashboard>();
    }

    public static IResult ToggleFeatureFlag(ToggleFeatureFlagRequest request)
    {
        return Component<AdminDashboard>();
    }
}

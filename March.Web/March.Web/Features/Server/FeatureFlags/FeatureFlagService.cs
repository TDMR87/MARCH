namespace March.Web.Features.Server.FeatureFlags;

public class FeatureFlagService(IConfiguration configuration)
{
    public bool IsFeatureEnabled(FeatureFlag featureFlag)
    {
        return configuration.GetValue($"Features:{featureFlag}:Enabled", defaultValue: false);
    }

    public bool IsFeatureDisabled(FeatureFlag featureFlag)
    {
        var enabled = configuration.GetValue($"Features:{featureFlag}:Enabled", defaultValue: true);
        return !enabled;
    }
}

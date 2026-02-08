namespace March.Web.Utils;

public static class Extensions
{
    public static RouteHandlerBuilder WithValidation<TValidator>(this RouteHandlerBuilder routeBuilder) 
        where TValidator : IEndpointFilter => routeBuilder.AddEndpointFilter<TValidator>();

    public static RouteHandlerBuilder WithFeatureFlags(
        this RouteHandlerBuilder routeBuilder, 
        params FeatureFlag[] featureFlags)
    {
        return routeBuilder.AddEndpointFilter(async (invocationContext, next) =>
        {
            var featureFlagService = invocationContext.HttpContext.RequestServices.GetRequiredService<FeatureFlagService>();
            var logger = invocationContext.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

            foreach (var featureFlag in featureFlags)
            {
                if (featureFlagService.IsFeatureDisabled(featureFlag))
                {
                    logger.LogWarning("{Feature} feature is disabled", featureFlag);
                    return Results.Problem($"{featureFlag} feature is disabled");
                }
            }

            return await next(invocationContext);
        });
    }

    public static bool TryGetError(
        this ValidationContext validationContext, 
        string PropertyName, 
        out ValidationFailure error)
    {
        if (PropertyName is null)
        {
            error = null!;
            return false;
        }

        error = validationContext.Errors.FirstOrDefault(x => x.PropertyName == PropertyName)!;
        return error is not null;
    }
}

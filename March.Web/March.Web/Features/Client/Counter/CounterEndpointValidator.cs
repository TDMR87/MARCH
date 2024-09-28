namespace March.Web.Features.Client.Counter;

public class CounterEndpointValidator(ILogger<CounterEndpointValidator> logger, FeatureFlagService featureFlags) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var requestPath = context.HttpContext.Request.Path.ToString();
        var model = context.Arguments.OfType<CounterEndpoint.Request>().FirstOrDefault();

        if (model == null)
        {
            logger.LogError("An invalid model was provided. {Model}", model);
            return Results.BadRequest("Invalid request payload.");
        }

        if ("/counter".In(requestPath) && featureFlags.IsFeatureDisabled(FeatureFlag.Counter))
        {
            logger.LogWarning("Feature {Feature} is disabled", FeatureFlag.Counter);
            return Results.Problem($"{FeatureFlag.Counter} feature is disabled");
        }

        if ("/counter/increment".In(requestPath) && featureFlags.IsFeatureDisabled(FeatureFlag.CounterIncrement))
        {
            logger.LogWarning("Feature {Feature} is disabled", FeatureFlag.CounterIncrement);
            return Results.Problem($"{FeatureFlag.CounterIncrement} feature is disabled");
        }
        if ("/counter/increment".In(requestPath) && model.Count > 10)
        {
            logger.LogError("Count cannot be greater than 10 (was {Count})", model.Count);
            return Results.BadRequest("Count cannot be greater than 10.");
        }

        if ("/counter/decrement".In(requestPath) && featureFlags.IsFeatureDisabled(FeatureFlag.CounterDecrement))
        {
            logger.LogWarning("Feature {Feature} is disabled", FeatureFlag.CounterDecrement);
            return Results.Problem($"{FeatureFlag.CounterDecrement} feature is disabled");
        }
        if ("/counter/decrement".In(requestPath) && model.Count < -10)
        {
            logger.LogError("Count cannot be less than -10 (was {Count})", model.Count);
            return Results.BadRequest("Count cannot be less than -10.");
        }

        return await next(context);
    }
}
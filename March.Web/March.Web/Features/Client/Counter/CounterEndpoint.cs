namespace March.Web.Features.Client.Counter;

public class CounterEndpoint
{
    public record Request(int Count);

    public static IResult Initialize(FeatureFlagService featureFlags)
    {
        if (featureFlags.IsFeatureDisabled(FeatureFlag.Counter))
        {
            return Results.Problem($"Feature '{FeatureFlag.Counter}' is disabled");
        }
        else return Component<CounterComponent, CounterComponentModel>(model: new()
        {
            Count = 0,
            Increment = true
        });
    }

    public static IResult Increment(Request request, FeatureFlagService featureFlags)
    {
        if (featureFlags.IsFeatureDisabled(FeatureFlag.CounterIncrement))
        {
            return Results.Problem($"Feature '{FeatureFlag.CounterIncrement}' is disabled");
        }
        else return Component<CounterComponent, CounterComponentModel>(model: new()
        {
            Count = request.Count,
            Increment = true
        });
    }

    public static IResult Decrement(Request request, FeatureFlagService featureFlags)
    {
        if (featureFlags.IsFeatureDisabled(FeatureFlag.CounterDecrement))
        {
            return Results.Problem($"Feature '{FeatureFlag.CounterDecrement}' is disabled");
        }
        else return Component<CounterComponent, CounterComponentModel>(model: new()
        {
            Count = request.Count,
            Increment = false
        });
    }

    public class RequestValidator
    {
        public RequestValidator()
        {
            // TODO: add FluentValidation   
        }
    }
}

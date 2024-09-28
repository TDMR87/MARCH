namespace March.Web.Features.Client.Counter;

public class CounterEndpoint
{
    public record Request(int Count);

    public static IResult Initialize(FeatureFlagService featureFlags)
    {
        return Component<CounterComponent, CounterComponentModel>(model: new()
        {
            Count = 0,
            Increment = true
        });
    }

    public static IResult Increment(Request request, FeatureFlagService featureFlags)
    {
        return Component<CounterComponent, CounterComponentModel>(model: new()
        {
            Count = request.Count,
            Increment = true
        });
    }

    public static IResult Decrement(Request request, FeatureFlagService featureFlags)
    {
        return Component<CounterComponent, CounterComponentModel>(model: new()
        {
            Count = request.Count,
            Increment = false
        });
    }

    public class CounterValidator
    {
        public CounterValidator()
        {
            // TODO: add FluentValidation   
        }
    }
}

namespace March.Web.Features.Client.Counter;

public class CounterEndpoint
{
    public record Request(int Count);

    public static IResult GetCounter(FeatureFlagService featureFlags)
    {
        return Component<Counter, CounterModel>(model: new()
        {
            Count = 0,
            IsIncrement = true
        });
    }

    public static IResult IncrementCounter(Request request, FeatureFlagService featureFlags)
    {
        return Component<Counter, CounterModel>(model: new()
        {
            IsIncrement = true,
            Count = request.Count,
        });
    }

    public static IResult DecrementCounter(Request request, FeatureFlagService featureFlags)
    {
        return Component<Counter, CounterModel>(model: new()
        {
            IsIncrement = false,
            Count = request.Count,
        });
    }
}

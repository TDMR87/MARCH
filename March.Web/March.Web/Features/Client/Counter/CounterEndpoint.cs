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

    public static IResult IncrementCounter(HttpContext httpContext, Request request, FeatureFlagService featureFlags)
    {
        var validationResult = httpContext.GetValidationResult();

        return Component<Counter, CounterModel>(model: new()
        {
            IsIncrement = true,
            Count = request.Count,
            Errors = validationResult.Errors.Select(e => (e.PropertyName, e.ErrorMessage)).ToList()
        });
    }

    public static IResult DecrementCounter(HttpContext httpContext, Request request, FeatureFlagService featureFlags)
    {
        var validationResult = httpContext.GetValidationResult();

        return Component<Counter, CounterModel>(model: new()
        {
            IsIncrement = false,
            Count = request.Count,
            Errors = validationResult.Errors.Select(e => (e.PropertyName, e.ErrorMessage)).ToList()
        });
    }
}

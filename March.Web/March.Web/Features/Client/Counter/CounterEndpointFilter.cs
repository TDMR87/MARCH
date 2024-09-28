namespace March.Web.Features.Client.Counter;

public class CounterEndpointFilter(ILogger<CounterEndpointFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        logger.LogInformation("Counter was clicked");
        return await next(context);
    }
}
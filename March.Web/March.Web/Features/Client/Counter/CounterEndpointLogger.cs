namespace March.Web.Features.Client.Counter;

public class CounterEndpointLogger(ILogger<CounterEndpointLogger> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        logger.LogInformation("Counter executed");
        return await next(context);
    }
}
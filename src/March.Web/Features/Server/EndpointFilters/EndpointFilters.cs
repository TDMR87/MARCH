namespace March.Web.Features.Server.EndpointFilters;

public class RequestLoggingFilter(ILogger<RequestLoggingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        logger.LogInformation("{Time} :: HTTP {Method} {Path} {ContentType}",
            DateTime.UtcNow.ToLocalTime(),
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            context.HttpContext.Request.ContentType);

        return await next(context);
    }
}

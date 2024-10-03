namespace March.Web.Features.Server.Exceptions;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger, 
    IHostEnvironment environment)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "text/html";

        await Component<GlobalException, GlobalExceptionModel>
        (
            model: new() 
            { 
                Message = environment.IsProduction()
                    ? "Something went wrong."
                    : exception.ToString()
            }
        ).ExecuteAsync(httpContext);

        return true;
    }
}

public static class GlobalExceptionHandlingExtensions
{
    public static void AddGlobalExceptionHandlingFeature(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();
    }

    public static IApplicationBuilder UseGlobalExceptionHandlingFeature(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler();
    }
}

namespace March.Web.Utils;

public class ValidatorBase<TValidator, TRequest>(ILogger<TValidator> logger, ValidationContext validationContext) 
    : AbstractValidator<TRequest>, IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        logger.LogInformation("Validating {Request}", typeof(TRequest));

        var requestModel = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (requestModel == null)
        {
            logger.LogError("Request contained an invalid request model. {Model}", requestModel);
            return Results.Problem("Invalid request model");
        }

        var validationResult = await ValidateAsync(requestModel);
        validationContext.ValidationResults = validationResult;

        return await next(context);
    }
}
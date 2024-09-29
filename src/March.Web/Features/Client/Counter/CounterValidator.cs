namespace March.Web.Features.Client.Counter;

public class CounterRulesValidator : AbstractValidator<CounterEndpoint.Request>
{
    public CounterRulesValidator()
    {
        RuleFor(x => x.Count)
            .InclusiveBetween(-5, 5)
            .WithMessage("Value must be between -5 and 5.");
    }
}

public class CounterValidator(
    ILogger<CounterValidator> logger, 
    IValidator<CounterEndpoint.Request> validator,
    ValidationContext validationContext) 
    : ValidatorBase<CounterEndpoint.Request>(logger, validator, validationContext);
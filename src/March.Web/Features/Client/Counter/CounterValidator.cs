namespace March.Web.Features.Client.Counter;

public class CounterValidator : ValidatorBase<CounterValidator, CounterEndpoint.Request>
{
    public CounterValidator(ILogger<CounterValidator> logger, ValidationContext validationContext)
        : base(logger, validationContext)
    {
        RuleFor(x => x.Count)
            .InclusiveBetween(-5, 5)
            .WithMessage("Value must be between -5 and 5.");
    }
}
namespace March.Web.Features.Client.Counter.UpdateCounter;

public class Validator : ValidatorBase<Validator, Request>
{
    public Validator(ILogger<Validator> logger, ValidationContext validationContext)
        : base(logger, validationContext)
    {
        RuleFor(x => x.Count)
            .InclusiveBetween(-5, 5)
            .WithMessage("Value must be between -5 and 5.");
    }
}

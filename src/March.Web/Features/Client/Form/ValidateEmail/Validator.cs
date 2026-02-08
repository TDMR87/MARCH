namespace March.Web.Features.Client.Form.ValidateEmail;

public class Validator : ValidatorBase<Validator, Request>
{
    public Validator(ILogger<Validator> logger, ValidationContext validationContext) : base(logger, validationContext)
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Not a valid email address");
    }
}
namespace March.Web.Features.Client.Form.SubmitForm;

public class Validator : ValidatorBase<Validator, Request>
{
    public Validator(ILogger<Validator> logger, ValidationContext validationContext)
        : base(logger, validationContext)
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required");

        RuleFor(x => x.Email)
           .NotEmpty()
           .WithMessage("Email is required")
           .EmailAddress()
           .WithMessage("Not a valid email address");
    }
}
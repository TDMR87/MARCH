namespace March.Web.Features.Client.Form;

public class EmailValidator : ValidatorBase<EmailValidator, FormEndpoint.EmailValidationRequest>
{
    public EmailValidator(ILogger<EmailValidator> logger, ValidationContext validationContext) 
        : base(logger, validationContext)
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Not a valid email address");
    }
}
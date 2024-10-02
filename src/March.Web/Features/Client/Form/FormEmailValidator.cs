namespace March.Web.Features.Client.Form;

public class FormEmailValidator : AbstractValidator<FormEndpoint.EmailValidationRequest>
{
    public FormEmailValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Not a valid email address");
    }
}

public class EmailValidator(
    ILogger<EmailValidator> logger,
    IValidator<FormEndpoint.EmailValidationRequest> validator,
    ValidationContext validationContext)
    : ValidatorBase<FormEndpoint.EmailValidationRequest>(logger, validator, validationContext);

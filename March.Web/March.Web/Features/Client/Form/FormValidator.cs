namespace March.Web.Features.Client.Form;

public class FormFieldsValidator : AbstractValidator<FormEndpoint.FormSubmitRequest>
{
    public FormFieldsValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required");

        RuleFor(x => x.Email)
           .NotEmpty()
           .WithMessage("Email is required");

        RuleFor(x => x.Email)
           .EmailAddress()
           .WithMessage("Not a valid email address");
    }
}

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

public class FormValidator(
    ILogger<FormValidator> logger, 
    IValidator<FormEndpoint.FormSubmitRequest> validator) 
    : ValidatorBase<FormEndpoint.FormSubmitRequest>(logger, validator);

public class EmailValidator(
    ILogger<EmailValidator> logger, 
    IValidator<FormEndpoint.EmailValidationRequest> validator) 
    : ValidatorBase<FormEndpoint.EmailValidationRequest>(logger, validator);
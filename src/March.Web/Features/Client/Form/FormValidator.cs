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
           .WithMessage("Email is required")
           .EmailAddress()
           .WithMessage("Not a valid email address");
    }
}

public class FormValidator(
    ILogger<FormValidator> logger,
    IValidator<FormEndpoint.FormSubmitRequest> validator,
    ValidationContext validationContext)
    : ValidatorBase<FormEndpoint.FormSubmitRequest>(logger, validator, validationContext);
namespace March.Web.Features.Client.Form;

public class FormValidator : ValidatorBase<FormValidator, FormEndpoint.FormSubmitRequest>
{
    public FormValidator(ILogger<FormValidator> logger, ValidationContext validationContext)
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
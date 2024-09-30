namespace March.Web.Features.Client.Authentication;

public class LoginFormValidator : AbstractValidator<LoginEndpoint.LoginRequest>
{
    public LoginFormValidator()
    {
        RuleFor(x => x.Username)
            .Equal("admin")
            .WithMessage("Username is not valid");

        RuleFor(x => x.Password)
            .Equal("test")
            .WithMessage("Password is not valid");
    }
}

public class LoginValidator(
ILogger<LoginValidator> logger,
IValidator<LoginEndpoint.LoginRequest> validator,
ValidationContext validationContext)
: ValidatorBase<LoginEndpoint.LoginRequest>(logger, validator, validationContext);

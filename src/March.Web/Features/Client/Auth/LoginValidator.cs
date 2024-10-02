namespace March.Web.Features.Client.Authentication;

public class LoginValidator : ValidatorBase<LoginValidator, LoginEndpoint.LoginRequest>
{
    public LoginValidator(ILogger<LoginValidator> logger, ValidationContext validationContext)
        : base(logger, validationContext)
    {
        RuleFor(x => x.Username)
            .Equal("admin")
            .WithMessage("Username is not valid");

        RuleFor(x => x.Password)
            .Equal("test")
            .WithMessage("Password is not valid");
    }
}

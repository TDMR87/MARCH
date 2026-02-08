namespace March.Web.Features.Client.Auth.SubmitLogin;

public class Validator : ValidatorBase<Validator, Request>
{
    public Validator(ILogger<Validator> logger, ValidationContext validationContext)
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

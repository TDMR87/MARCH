namespace March.Web.Features.Client.Auth.SubmitLogin;

public record Request(
    string Username, 
    string Password, 
    string? ReturnUrl = null
);

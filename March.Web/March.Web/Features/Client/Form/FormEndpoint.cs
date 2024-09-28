namespace March.Web.Features.Client.Form;

public class FormEndpoint
{
    public record FormSubmitRequest(string? Name, string? Email);
    public record EmailValidationRequest(string Email);

    public static IResult GetForm()
    {
        return Component<Form>();
    }

    public static IResult SubmitForm(HttpContext httpContext, FormSubmitRequest request)
    {
        var validationResult = httpContext.GetValidationResult();

        return Component<Form, FormModel>(model: new()
        {
            Name = request.Name,
            Email = request.Email,
            Errors = validationResult.Errors.Select(e => (e.PropertyName, e.ErrorMessage)).ToList()
        });
    }

    public static IResult ValidateEmail(HttpContext httpContext, EmailValidationRequest request)
    {
        var validationResult = httpContext.GetValidationResult();

        return Component<FormEmail, FormModel>(model: new()
        {
            Email = request.Email,
            Errors = validationResult.Errors.Select(e => (e.PropertyName, e.ErrorMessage)).ToList()
        });
    }
}

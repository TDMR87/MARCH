namespace March.Web.Features.Client.Form;

public class FormEndpoint
{
    public record FormSubmitRequest(string? Name, string? Email);
    public record EmailValidationRequest(string Email);

    public static IResult GetForm()
    {
        return Component<Form>();
    }

    public static IResult SubmitForm(FormSubmitRequest request)
    {
        return Component<Form, FormModel>(model: new()
        {
            Name = request.Name,
            Email = request.Email,
        });
    }

    public static IResult ValidateEmail(EmailValidationRequest request)
    {
        return Component<FormEmail, FormModel>(model: new()
        {
            Email = request.Email,
        });
    }
}

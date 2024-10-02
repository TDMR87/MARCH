namespace March.Web.Features.Client.Form;

public class FormEndpoint
{
    public record FormSubmitRequest(string? Name, string? Email);
    public record EmailValidationRequest(string Email);
    public record FormSubmittedModalRequest(bool Toggle);

    public static IResult GetForm()
    {
        return Component<Form>();
    }

    public static IResult SubmitForm(FormSubmitRequest request, ValidationContext validationContext)
    {
        if (validationContext.Errors.Any())
        {
            return Component<Form, FormModel>(model: new()
            {
                Name = request.Name,
                Email = request.Email,
            });
        }

        return Component<FormSubmitted>();
    }

    public static IResult ValidateEmail(EmailValidationRequest request)
    {
        return Component<FormEmail, FormModel>(model: new()
        {
            Email = request.Email,
        });
    }

    public static IResult ToggleFormSubmittedModal(FormSubmittedModalRequest request)
    {
        if (request.Toggle == false)
        {
            return Component<Form>();
        }
        else
        {
            return Component<FormSubmitted, FormSubmittedModel>(new() { Toggle = true });
        }
    }
}

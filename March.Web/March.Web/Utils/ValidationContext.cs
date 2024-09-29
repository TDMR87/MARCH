namespace March.Web.Utils;

public class ValidationContext
{
    public ValidationResult ValidationResults { get; set; } = new();

    public List<ValidationFailure> Errors => ValidationResults.Errors;
}

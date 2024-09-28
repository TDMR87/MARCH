namespace March.Web.Features.Client.HowItWorks;

public class HowItWorksEndpoint
{
    public record Request();

    public static IResult GetHowItWorks()
    {
        return Component<HowItWorks>();
    }
}

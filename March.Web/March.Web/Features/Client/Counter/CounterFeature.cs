namespace March.Web.Features.Client.Counter;

public class CounterFeature : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("counter", HandleRequest)
        .AllowAnonymous()
        .WithSummary("Increments counter");

    public record Request(int Count);
    public record Response(string Token);

    public class RequestValidator
    {
        public RequestValidator()
        {
            // TODO: add FluentValidation   
        }
    }

    private static RazorComponentResult HandleRequest(
        CounterService counterService,
        Request request,
        CancellationToken cancellationToken)
    {
        return Component<Counter, CounterButtonModel>(model: new()
        {
            Count = request.Count
        });
    }
}

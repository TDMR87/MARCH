namespace March.Web.Features.Client.Counter;

public class CounterEndpoint
{
    public record Request(int Count);

    public static IResult GetCounter()
    {
        return Component<Counter>();
    }


    public static IResult UpdateCounter(Request request)
    {
        return Component<Counter, CounterModel>(model: new()
        {
            Count = request.Count,
        });
    }
}
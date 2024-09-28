using System.Text.Json;

namespace March.Web.Features.Client.Counter;

public record CounterComponentModel
{
    public int Count { get; set; }
    public bool Increment { get; set; }

    public string ToJson() => JsonSerializer.Serialize(this);
}

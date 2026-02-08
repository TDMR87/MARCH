using System.Text.Json;
using System.Text.Json.Serialization;

namespace March.Web.Features.Server.Serialization;

public static class Serialization
{
    public static void AddJsonSerializationFeature(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }
}

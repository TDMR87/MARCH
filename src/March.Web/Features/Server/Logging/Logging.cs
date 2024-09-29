namespace March.Web.Features.Server.Logging;

public static class Logging
{
    public static void AddLoggingFeature(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
    }
}

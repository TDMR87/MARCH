namespace March.Web.Features.Server.ResponseCompression;

public static class ResponseCompression
{
    public static void AddResponseCompressionFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            [
                "text/html",
                "application/json",
                "text/css",
                "application/javascript"
            ]);
        });
    }
}

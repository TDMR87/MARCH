namespace March.Web.Features.Server.Middleware;

public class HtmxRequestMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request has the HX-Request header
        var isHtmxRequest = context.Request.Headers.ContainsKey("hx-request");

        if (!isHtmxRequest)
        {
            // If it's not an HTMX request, serve index.html
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync("Features/Server/StaticFiles/index.html");
            return;
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
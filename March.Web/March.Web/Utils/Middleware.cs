namespace March.Web.Utils;

public class Redirect404Middleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404)
        {
            context.Response.Redirect("/");
        }
    }
}
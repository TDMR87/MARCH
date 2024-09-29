namespace March.Web.Features.Server.Authentication;

public static class Auth
{
    public static void AddAuthFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("MarchCookieAuth")
            .AddCookie("MarchCookieAuth", options =>
            {
                options.LoginPath = "/login"; // Redirect to this path for login
                options.AccessDeniedPath = "/"; // Redirect to this path if access is denied
            });

        builder.Services.AddAuthorization();
    }

    public static IApplicationBuilder UseAuthFeature(this IApplicationBuilder app)
    {
        return app
            .UseAuthentication()
            .UseAuthorization();
    }
}

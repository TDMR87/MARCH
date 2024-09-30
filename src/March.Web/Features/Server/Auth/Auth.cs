namespace March.Web.Features.Server.Auth;

public static class Auth
{
    public static void AddAuthFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(Constants.MarchAuthCookie)
            .AddCookie(Constants.MarchAuthCookie, options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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

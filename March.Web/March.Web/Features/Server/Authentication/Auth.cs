namespace March.Web.Features.Server.Authentication;

public static class Auth
{
    public static void AddAuthFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
    }

    public static IApplicationBuilder UseAuthFeature(this IApplicationBuilder app)
    {
        return app
            .UseAuthentication()
            .UseAuthorization();
    }
}

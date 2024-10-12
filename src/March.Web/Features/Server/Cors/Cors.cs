namespace March.Web.Features.Server.Cors;

public static class Cors
{
    public static void AddCorsFeature(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCors(policy => policy
            .AddDefaultPolicy(builder => builder
            .WithOrigins("https://localhost:5001", "https://march-project.azurewebsites.net/")
            .AllowAnyHeader()
            .AllowAnyMethod()));
    }

    public static IApplicationBuilder UseCorsFeature(this IApplicationBuilder app)
    {
        return app.UseCors();
    }
}

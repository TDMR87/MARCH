namespace March.Web.Features.Server.Rendering;

public static class RenderingFeature
{
    public static void AddServerSideRenderingFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents();
    }
}

namespace March.Web.Features.Server.StaticFiles;

public static class StaticFiles
{
    public static IApplicationBuilder UseStaticFilesFeature(this IApplicationBuilder app)
    {
        return app
            .UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = ["index.html"]})
            .UseStaticFiles();
    }
}

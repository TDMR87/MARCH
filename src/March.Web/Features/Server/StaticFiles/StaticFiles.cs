namespace March.Web.Features.Server.StaticFiles;

public static class StaticFiles
{
    public static WebApplicationBuilder CreateMarchAppBuilder(string[] args)
    {
        return WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            WebRootPath = "Features/Server/StaticFiles",
        });
    }

    public static IApplicationBuilder UseStaticFilesFeature(this IApplicationBuilder app)
    {
        return app
            .UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = ["index.html"]})
            .UseStaticFiles();
    }
}

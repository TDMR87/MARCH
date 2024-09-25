namespace March.Web.Utils;

public static class Extensions
{
    public interface IEndpoint
    {
        static abstract void Map(IEndpointRouteBuilder app);
    }

    public static RazorComponentResult Component<TComponent, TModel>(TModel? model = default) where TComponent : IComponent
    {
        return model is null
            ? new RazorComponentResult<TComponent>()
            : new RazorComponentResult<TComponent>(new { Model = model });
    }

    public static RazorComponentResult Component<TComponent>() where TComponent : IComponent
    {
        return new RazorComponentResult<TComponent>();
    }

    public static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}

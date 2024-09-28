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

    public static IEndpointRouteBuilder AddPublicFeature(this WebApplication app)
    {
        return app.MapGroup("").AllowAnonymous();
    }

    public static IEndpointRouteBuilder AddPrivateFeature(this WebApplication app)
    {
        return app.MapGroup("").RequireAuthorization();
    }

    public static IEndpointRouteBuilder WithFeatureFlags(this IEndpointRouteBuilder app, params FeatureFlag[] featureFlags)
    {
        return app;
    }

    public static RouteHandlerBuilder WithRoutePath(
        this IEndpointRouteBuilder routeBuilder,
        HttpMethod httpMethod,
        string routePath,
        Delegate endpointHandler)
    {
        return httpMethod switch
        {
            HttpMethod.Get => routeBuilder.MapGet(routePath, endpointHandler),
            HttpMethod.Post => routeBuilder.MapPost(routePath, endpointHandler),
            HttpMethod.Put => routeBuilder.MapPut(routePath, endpointHandler),
            HttpMethod.Delete => routeBuilder.MapDelete(routePath, endpointHandler),
            _ => throw new NotSupportedException($"The specified Http method {httpMethod} is not supported")
        };
    }
}

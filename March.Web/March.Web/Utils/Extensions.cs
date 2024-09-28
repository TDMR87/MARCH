using Microsoft.AspNetCore.Http;

namespace March.Web.Utils;

public static class Extensions
{
    public static IEndpointRouteBuilder AddPublicFeature(this RouteGroupBuilder routeBuilder) =>  routeBuilder
        .AllowAnonymous();

    public static IEndpointRouteBuilder AddPrivateFeature(this RouteGroupBuilder routeBuilder) => routeBuilder
        .RequireAuthorization();

    public static RouteHandlerBuilder WithValidation<TValidator>(this RouteHandlerBuilder routeBuilder) where TValidator : IEndpointFilter
    {
        return routeBuilder.AddEndpointFilter<TValidator>();
    }

    public static IEndpointRouteBuilder WithFeatureFlags(this IEndpointRouteBuilder app, params FeatureFlag[] featureFlags)
    {
        return app;
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

    public static bool In(this string text, string source) => source.Contains(text);

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
            _ => throw new NotSupportedException($"Http method '{httpMethod}' is not supported")
        };
    }
}



var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Features/Server/StaticFiles",
});

// Add features
builder.AddLoggingFeature();
builder.AddCorsFeature();
builder.AddAuthFeature();
builder.AddResponseCompressionFeature();
builder.AddServerSideRenderingFeature();

// Add services
builder.Services.AddSingleton<CounterService>();

var app = builder.Build();

// Use features
app.UseCorsFeature();
app.UseStaticFilesFeature();
app.UseHttpsRedirection();
app.UseAuthFeature();

// Use middleware
app.UseMiddleware<Redirect404Middleware>();

// Map features to API endpoints
var endpoints = app.MapGroup("")
   .RequireAuthorization()
   .AddEndpointFilter<RequestLoggingFilter>()
   .MapEndpoint<HomeFeature>()
   .MapEndpoint<NavMenuFeature>()
   .MapEndpoint<FormFeature>()
   .MapEndpoint<UserProfileFeature>()
   .MapEndpoint<CounterFeature>();

// Bind the root App component
app.MapGet("app", () => Component<App>());
app.Run();
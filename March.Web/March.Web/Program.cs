var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Features/Server/StaticFiles",
});

// Add server features
builder.AddLoggingFeature();
builder.AddCorsFeature();
builder.AddAuthFeature();
builder.AddResponseCompressionFeature();
builder.AddServerSideRenderingFeature();

// Add services
builder.Services.AddSingleton<FeatureFlagService>();

var app = builder.Build();

// Use server features
app.UseCorsFeature();
app.UseStaticFilesFeature();
app.UseHttpsRedirection();
app.UseAuthFeature();

// Use middleware
app.UseMiddleware<Redirect404Middleware>();
app.MapFallbackToFile("index.html");

// Add application features
var features = app.MapGroup("")
    .AddEndpointFilter<RequestLoggingFilter>();

features.AddPublicFeature()
   .WithRoutePath(HttpMethod.Post, "/nav", NavMenuFeature.HandleRequest)
   .WithSummary("A navigation menu");

features.AddPublicFeature()
   .WithRoutePath(HttpMethod.Get, "/home", HomeFeature.HandleRequest)
   .WithSummary("Get home page content");

features.AddPublicFeature()
   .WithRoutePath(HttpMethod.Get, "counter", CounterEndpoint.Initialize)
   .AddEndpointFilter<CounterEndpointLogger>()
   .WithSummary("Initializes a counter button");

features.AddPublicFeature()
   .WithRoutePath(HttpMethod.Post, "counter/increment", CounterEndpoint.Increment)
   .AddEndpointFilter<CounterEndpointLogger>()
   .WithValidation<CounterEndpointValidator>()
   .WithSummary("Increments a counter button");

features.AddPublicFeature()
   .WithRoutePath(HttpMethod.Post, "counter/decrement", CounterEndpoint.Decrement)
   .AddEndpointFilter<CounterEndpointLogger>()
   .WithValidation<CounterEndpointValidator>()
   .WithSummary("Decrements a counter button");

features.AddPublicFeature()
   .WithRoutePath(HttpMethod.Get, "/profile", UserProfileFeature.HandleRequest)
   .WithSummary("Get user profile");

// Bind the root App component
app.MapGet("app", () => Component<App>());
app.Run();
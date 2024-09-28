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

//app.MapGroup("")
//    .AddEndpointFilter<RequestLoggingFilter>();

// Use client features
app.AddPublicFeature()
   .WithRoutePath(HttpMethod.Get, "counter", CounterEndpoint.Initialize)
   .AddEndpointFilter<CounterEndpointFilter>()
   .WithSummary("Initialize a counter button");

app.AddPublicFeature()
   .WithRoutePath(HttpMethod.Post, "counter/increment", CounterEndpoint.Increment)
   .AddEndpointFilter<CounterEndpointFilter>()
   .WithSummary("Increment a counter button");

app.AddPublicFeature()
   .WithRoutePath(HttpMethod.Post, "counter/decrement", CounterEndpoint.Decrement)
   .AddEndpointFilter<CounterEndpointFilter>()
   .WithSummary("Decrement a counter button");

app.AddPublicFeature()
   .WithRoutePath(HttpMethod.Get, "/home", HomeFeature.HandleRequest)
   .WithSummary("Get home page content");

app.AddPublicFeature()
   .WithRoutePath(HttpMethod.Post, "/nav", NavMenuFeature.HandleRequest)
   .WithSummary("Open / close a navigation menu");

app.AddPublicFeature()
   .WithRoutePath(HttpMethod.Get, "/profile", UserProfileFeature.HandleRequest)
   .WithSummary("Get user profile");

// Bind the root App component
app.MapGet("app", () => Component<App>());
app.Run();
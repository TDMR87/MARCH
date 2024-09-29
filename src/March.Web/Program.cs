using March.Web.Features.Client.AdminDashboard;
using March.Web.Features.Client.Authentication;

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
builder.Services.AddScoped<ValidationContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<FeatureFlagService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Use server features
var app = builder.Build();
app.UseCorsFeature();
app.UseStaticFilesFeature();
app.UseHttpsRedirection();
app.UseAuthFeature();
app.MapFallbackToFile("index.html");

// Add global request filters etc.
var features = app
    .MapGroup("")
    .AddEndpointFilter<RequestLoggingFilter>();

// Add application features
features.AddPublicFeature()
        .WithRoutePath(HTTP.GET, "/home", HomeEndpoint.GetHome)
        .WithSummary("Get the home page content");

features.AddPublicFeature()
        .WithRoutePath(HTTP.GET, "/nav", NavEndpoint.GetNav)
        .WithSummary("Get the navigation menu");

features.AddPublicFeature()
        .WithRoutePath(HTTP.POST, "/nav", NavEndpoint.ToggleNav)
        .WithSummary("Open / close a navigation menu");

features.AddPublicFeature()
        .WithRoutePath(HTTP.GET, "/how-it-works", HowItWorksEndpoint.GetHowItWorks)
        .WithSummary("Get a how-it-works page");

features.AddPublicFeature()
        .WithRoutePath(HTTP.GET, "/counter", CounterEndpoint.GetCounter)
        .WithFeatureFlags(FeatureFlag.Counter)
        .WithSummary("Get a counter button");

features.AddPublicFeature()
        .WithRoutePath(HTTP.POST, "/counter/increment", CounterEndpoint.IncrementCounter)
        .WithFeatureFlags(FeatureFlag.CounterIncrement)
        .WithValidation<CounterValidator>()
        .WithSummary("Increment a counter button");

features.AddPublicFeature()
        .WithRoutePath(HTTP.POST, "/counter/decrement", CounterEndpoint.DecrementCounter)
        .WithFeatureFlags(FeatureFlag.CounterDecrement)
        .WithValidation<CounterValidator>()
        .WithSummary("Decrement a counter button");

features.AddPublicFeature()
        .WithRoutePath(HTTP.GET, "/form", FormEndpoint.GetForm)
        .WithFeatureFlags(FeatureFlag.Form)
        .WithSummary("Get an input form");

features.AddPublicFeature()
        .WithRoutePath(HTTP.POST, "/form/submit", FormEndpoint.SubmitForm)
        .WithFeatureFlags(FeatureFlag.FormSubmit)
        .WithValidation<FormValidator>()
        .WithSummary("Submit an input form");

features.AddPublicFeature()
        .WithRoutePath(HTTP.POST, "/form/email", FormEndpoint.ValidateEmail)
        .WithValidation<EmailValidator>()
        .WithSummary("Validate email");

features.AddPublicFeature()
        .WithRoutePath(HTTP.GET, "/login", LoginEndpoint.GetLogin)
        .WithSummary("Get login form");

features.AddPublicFeature()
        .WithRoutePath(HTTP.POST, "/login", LoginEndpoint.SubmitLogin)
        .WithValidation<LoginValidator>()
        .WithSummary("Login");

features.AddPublicFeature()
        .WithRoutePath(HTTP.GET, "/logout", LogoutEndpoint.Logout)
        .WithSummary("Log out");

features.AddProtectedFeature()
        .WithRoutePath(HTTP.GET, "/admin/dashboard", AdminEndpoint.GetDashboard)
        .WithSummary("Get the admin dashboard");

app.MapGet("app", () => Component<App>());
app.Run();
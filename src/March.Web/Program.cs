var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Features/Server/StaticFiles",
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new StringToBooleanConverter());
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

// Add server features
builder.AddLoggingFeature();
builder.AddCorsFeature();
builder.AddAuthFeature();
builder.AddResponseCompressionFeature();
builder.AddServerSideRenderingFeature();
builder.AddGlobalExceptionHandlingFeature();

builder.Services.AddScoped<ValidationContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<FeatureFlagService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Use server features
var app = builder.Build();
app.UseGlobalExceptionHandlingFeature();
app.UseCorsFeature();
app.UseStaticFilesFeature();
app.UseHttpsRedirection();
app.UseAuthFeature();
app.UseMiddleware<HtmxRequestMiddleware>();

// Add global request filters etc.
var routes = app.MapGroup("")
   .AddEndpointFilter<RequestLoggingFilter>();

// Add routes
routes.AddRoutePath(HTTP.GET, "/notfound", RoutingEndpoint.NotFound)
      .AllowAnonymous();

routes.AddRoutePath(HTTP.GET, "/home", HomeEndpoint.GetHome)
      .Produces<RazorComponentResult<Home>>()
      .AllowAnonymous()
      .WithSummary("Get the home page content");

routes.AddRoutePath(HTTP.GET, "/nav-items", NavEndpoint.GetNavItems)
      .Produces<RazorComponentResult<NavItems>>()
      .AllowAnonymous()
      .WithSummary("Get the navigation menu");

routes.AddRoutePath(HTTP.GET, "/how-it-works", HowItWorksEndpoint.GetHowItWorks)
      .Produces<RazorComponentResult<HowItWorks>>()
      .AllowAnonymous()
      .WithSummary("Get a how-it-works page");

routes.AddRoutePath(HTTP.GET, "/counter", CounterEndpoint.GetCounter)
      .Produces<RazorComponentResult<Counter>>()
      .WithFeatureFlags(FeatureFlag.Counter)
      .AllowAnonymous()
      .WithSummary("Get a counter component");

routes.AddRoutePath(HTTP.POST, "/counter/update", CounterEndpoint.UpdateCounter)
      .Produces<RazorComponentResult<Counter>>()
      .WithFeatureFlags(FeatureFlag.Counter)
      .WithValidation<CounterValidator>()
      .AllowAnonymous()
      .WithSummary("Update a counter component");

routes.AddRoutePath(HTTP.GET, "/form", FormEndpoint.GetForm)
      .Produces<RazorComponentResult<Form>>()
      .WithFeatureFlags(FeatureFlag.Form)
      .AllowAnonymous()
      .WithSummary("Get an input form");

routes.AddRoutePath(HTTP.POST, "/form/submit", FormEndpoint.SubmitForm)
      .Produces<RazorComponentResult<Form>>()
      .WithFeatureFlags(FeatureFlag.FormSubmit)
      .WithValidation<FormValidator>()
      .AllowAnonymous()
      .WithSummary("Submit an input form");

routes.AddRoutePath(HTTP.POST, "/form/submitted", FormEndpoint.ToggleFormSubmittedModal)
      .Produces<RazorComponentResult<FormSubmitted>>()
      .WithFeatureFlags(FeatureFlag.FormSubmit)
      .AllowAnonymous()
      .WithSummary("Toggle a form submitted modal");

routes.AddRoutePath(HTTP.POST, "/form/email", FormEndpoint.ValidateEmail)
      .Produces<RazorComponentResult<FormEmail>>()
      .WithValidation<EmailValidator>()
      .AllowAnonymous()
      .WithSummary("Validate email");

routes.AddRoutePath(HTTP.GET, "/login", LoginEndpoint.GetLogin)
      .Produces<RazorComponentResult<Login>>()
      .AllowAnonymous()
      .WithSummary("Get login form");

routes.AddRoutePath(HTTP.POST, "/login", LoginEndpoint.SubmitLogin)
      .Produces<RedirectHttpResult>()
      .Produces<RazorComponentResult<Home>>()
      .WithValidation<LoginValidator>()
      .AllowAnonymous()
      .WithSummary("Login");

routes.AddRoutePath(HTTP.GET, "/logout", LogoutEndpoint.Logout)
      .Produces<RazorComponentResult<Home>>()
      .RequireAuthorization()
      .WithSummary("Log out");

routes.AddRoutePath(HTTP.GET, "/admin/dashboard", AdminEndpoint.GetDashboard)
      .Produces<RazorComponentResult<AdminDashboard>>()
      .RequireAuthorization()
      .WithSummary("Get the admin dashboard");

routes.AddRoutePath(HTTP.POST, "/admin/feature-toggle", AdminEndpoint.ToggleFeatureFlag)
      .Produces<RazorComponentResult<AdminDashboard>>()
      .RequireAuthorization()
      .WithSummary("Toggles a feature on or off");

app.MapGet("app", () => Component<App>());
app.Run();
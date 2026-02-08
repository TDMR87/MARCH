var builder = StaticFiles.CreateMarchAppBuilder(args);

// Add server-side features
builder.AddJsonSerializationFeature();
builder.AddLoggingFeature();
builder.AddCorsFeature();
builder.AddAuthFeature();
builder.AddResponseCompressionFeature();
builder.AddServerSideRenderingFeature();
builder.AddGlobalExceptionHandlingFeature();

// Dependency injection
builder.Services.AddScoped<ValidationContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<FeatureFlagService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Use server-side features
var app = builder.Build();
app.UseGlobalExceptionHandlingFeature();
app.UseCorsFeature();
app.UseStaticFilesFeature();
app.UseHttpsRedirection();
app.UseAuthFeature();
app.UseMiddleware<HtmxRequestMiddleware>();

// Add filters to all routes in the app
var routeGroup = app
    .MapGroup("")
    .AddEndpointFilter<RequestLoggingFilter>();

// Add client-side features
_ = new March.Web.Features.Client.Home.GetHome.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Nav.GetNav.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Auth.Logout.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Auth.GetLogin.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Auth.SubmitLogin.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Admin.GetDashboard.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Admin.ToggleFeatureFlag.Endpoint(routeGroup);
_ = new March.Web.Features.Client.HowItWorks.GetHowItWorks.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Counter.GetCounter.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Counter.UpdateCounter.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Form.GetForm.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Form.SubmitForm.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Form.FormSubmitted.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Form.ValidateEmail.Endpoint(routeGroup);
_ = new March.Web.Features.Client.NotFound.GetNotFound.Endpoint(routeGroup);

app.MapGet("app", () => new RazorComponentResult<App>());
app.Run();
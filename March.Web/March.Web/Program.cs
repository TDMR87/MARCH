var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = ""
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(policy => policy
    .AddDefaultPolicy(builder => builder
    .WithOrigins("https://localhost:5001")
    .AllowAnyHeader()
    .AllowAnyMethod()));

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    [
        "text/html",
        "application/json",
        "text/css",
        "application/javascript"
    ]);
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddRazorComponents();
builder.Services.AddSingleton<CounterService>();

var app = builder.Build();
app.UseCors();
app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = ["index.html"] });
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<Redirect404Middleware>();
app.MapGet("app", () => Component<App>());

var endpoints = app.MapGroup("")
   .RequireAuthorization()
   .AddEndpointFilter<RequestLoggingFilter>()
   .MapEndpoint<HomeEndpoint>()
   .MapEndpoint<CounterEndpoint>()
   .MapEndpoint<NavMenuEndpoint>()
   .MapEndpoint<ProfileEndpoint>();

app.Run();
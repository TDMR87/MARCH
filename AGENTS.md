# Agents Documentation

This file contains implementation details and conventions for AI coding agents working on the MARCH project.

## Development Instructions

**IMPORTANT**: Keep this file synchronized with the codebase.

- When making changes that affect project architecture, build process, conventions, or integrations, **update this file immediately**
- If you detect a desync between this documentation and the actual implementation, **correct this file first** before proceeding
- Document any non-obvious implementation details that would help AI agents understand the project better
- Keep information concise but complete - focus on "how" and "why" rather than just "what"

## Building the Project

### Prerequisites
- **.NET 10.0 SDK** - Required for building and running the ASP.NET Core application
- **Node.js & npm** - Required for Tailwind CSS compilation

### Build Commands

#### Full Build (Development)
```bash
cd src/March.Web
dotnet build
```
This command:
1. Restores NuGet packages (FluentValidation, etc.)
2. Runs the Tailwind CSS build process (via MSBuild target)
3. Compiles the .NET application
4. Copies static files to output directory

#### Run the Application
```bash
cd src/March.Web
dotnet run
```
- Application will start on the configured ports (check console output)
- By default uses `appsettings.json` and `appsettings.Development.json`
- Hot reload is enabled by default in .NET 9

#### Build for Production
```bash
cd src/March.Web
dotnet publish -c Release -o ./publish
```

### Initial Setup
First time working with the project:
```bash
cd src/March.Web
npm install          # Install Tailwind CSS
dotnet restore       # Restore NuGet packages
dotnet build         # Build the project
```

### Important Notes
- **npm install must be run first** before building (to install Tailwind CLI)
- The build process is integrated - running `dotnet build` automatically compiles Tailwind CSS
- Static files are served from `Features/Server/StaticFiles` (configured as WebRootPath)
- User secrets are configured (ID: `bf227e66-6990-4cd3-976c-d63dfd30ebb2`)

## Tailwind CSS Integration

### Build Process
Tailwind CSS is integrated into the .NET build pipeline through MSBuild:

**Location**: `src/March.Web/March.Web.csproj`
```xml
<Target Name="Tailwind" BeforeTargets="Build">
    <Exec Command="npm run css:build" />
</Target>
```

This automatically runs Tailwind compilation **before every .NET build**, ensuring CSS is always up-to-date.

### NPM Configuration
**Location**: `src/March.Web/package.json`
- **Dev Dependency**: `tailwindcss: ^3.4.13`
- **Build Script**: `npm run css:build`
  - Compiles: `App.css` → `Features/Server/StaticFiles/App.compiled.css`

### File Structure
- **Source CSS**: `src/March.Web/App.css`
  - Contains only Tailwind directives (`@tailwind base/components/utilities`)
  - Do not add custom CSS here unless following Tailwind's layer system

- **Compiled CSS**: `src/March.Web/Features/Server/StaticFiles/App.compiled.css`
  - Generated file (do not edit manually)
  - Automatically copied to output directory
  - Served at `/App.compiled.css` endpoint

- **Config**: `src/March.Web/tailwind.config.js`
  - **Content paths** (scanned for Tailwind classes):
    - `./*.razor` (root Razor files)
    - `./Features/Server/StaticFiles/*.html`
    - `./Features/Server/StaticFiles/*.js`
    - `./Features/Client/**/*.razor` (all feature components)
  - **Dark mode**: Configured as `'selector'` (uses `.dark` class)

### Integration Point
The compiled CSS is referenced in the main HTML template:

**Location**: `src/March.Web/Features/Server/StaticFiles/index.html`
```html
<link href="/App.compiled.css" rel="stylesheet">
```

### Important Notes
1. **No manual npm commands needed** - The build process handles everything
2. **Tailwind classes work in all Razor components** under the content paths
3. **Purging is automatic** - Unused styles are removed in production builds
4. **To add custom CSS**: Use Tailwind's `@layer` directives in `App.css`
5. **Dark mode**: Toggle by adding/removing `dark` class on `<html>` element

### Modifying Tailwind
- **Add new content paths**: Edit `tailwind.config.js` → `content` array
- **Add custom utilities**: Use `@layer utilities` in `App.css`
- **Extend theme**: Edit `tailwind.config.js` → `theme.extend`
- **Add plugins**: `npm install` plugin, then add to `plugins` array in config

---

## Project Architecture

### Vertical Slice Architecture
The project uses a **true vertical slice architecture** where each feature operation is completely self-contained. This means:

**Benefits**:
- **High cohesion**: All code for a single operation lives together
- **Low coupling**: Features don't share code unless explicitly needed
- **Easy navigation**: Finding code is intuitive - follow the folder structure
- **Self-documenting**: Structure reflects the HTTP API surface
- **Parallel development**: Teams can work on different features without conflicts
- **Easy to delete**: Removing a feature is just deleting its folder

### Structure Philosophy
Each feature operation is a self-contained folder with all its related code:
- Endpoint (HTTP handler with self-registration)
- Request DTO (if needed)
- Validator (if needed)
- Razor component (UI, in Components/ subfolder)

### Folder Structure
```
src/March.Web/
├── Features/
│   ├── Client/              # User-facing features
│   │   ├── Counter/         # Example: Counter feature
│   │   │   ├── Components/  # Shared components
│   │   │   │   └── Counter.razor
│   │   │   ├── GetCounter/  # GET operation
│   │   │   │   └── Endpoint.cs
│   │   │   └── UpdateCounter/  # POST operation
│   │   │       ├── Endpoint.cs
│   │   │       ├── Request.cs
│   │   │       └── Validator.cs
│   │   ├── Home/
│   │   ├── Auth/
│   │   └── ...
│   └── Server/              # Server features & static files
│       ├── StaticFiles/     # Web root (served as /)
│       └── ...
├── Utils/                   # Shared utilities
├── Program.cs               # Entry point & endpoint instantiation
├── App.razor                # Main layout component
└── _Imports.razor           # Global Razor imports
```

**Key Structure Principles**:
- Each HTTP operation (GET /counter, POST /counter/update) gets its own folder
- Shared components for a feature live in `{Feature}/Components/`
- Request DTOs and Validators are co-located with their endpoint
- Endpoints self-register via constructor injection

### Client vs Server Features
- **Client/**: Features that render UI and handle user interactions
- **Server/**: Infrastructure features (static files, middleware, etc.)

---

## Creating New Features

### Step-by-Step Guide: Creating a "Todo" Feature

#### 1. Create Feature Folder Structure
Location: `src/March.Web/Features/Client/Todo/`

```
Features/Client/Todo/
├── Components/
│   └── Todo.razor          # Shared UI component
├── GetTodo/                # GET operation
│   └── Endpoint.cs
└── UpdateTodo/             # POST operation
    ├── Endpoint.cs
    ├── Request.cs
    └── Validator.cs        # Optional
```

**Key Structure Rules**:
- Each HTTP operation gets its own folder (GetTodo, UpdateTodo, CreateTodo, etc.)
- Shared components live in `Components/` subfolder
- Request DTOs are in `Request.cs` (optional if no input needed)
- Validators are co-located with their endpoint

---

#### 2. Create Razor Component
**File**: `Components/Todo.razor`

```razor
<div id="todo">
    <!-- Your HTML with Tailwind classes -->
    <h1 class="text-2xl font-bold">@Title</h1>
    
    @if (ValidationContext.TryGetError(nameof(Title), out var error))
    {
        <p class="text-red-500">@error.ErrorMessage</p>
    }
</div>

@code {
    [Inject] 
    public ValidationContext ValidationContext { get; set; } = new();
    
    [Parameter] 
    public string Title { get; set; } = "";
}
```

**Key conventions**:
- Root element must have an `id` attribute (used for HTMX targeting)
- Use `[Parameter]` for component inputs
- Inject `ValidationContext` if validation errors need to be displayed
- Keep component logic minimal - handlers go in endpoints

---

#### 3. Create GET Endpoint
**File**: `GetTodo/Endpoint.cs`

```csharp
namespace March.Web.Features.Client.Todo.GetTodo;

public class Endpoint
{
    public const string Path = "/todo";
    private const string summary = "Get todo component";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapGet(Path, Handler)
            .Produces<RazorComponentResult<Components.Todo>>()
            .AllowAnonymous()  // or .RequireAuthorization()
            .WithSummary(summary);
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.Todo>();
    }
}
```

**Key conventions**:
- Class named `Endpoint` in operation-specific namespace
- Constructor takes `RouteGroupBuilder` and registers the route
- `const string Path` defines the route
- Static `Handler` method contains the logic
- Self-registration via constructor

---

#### 4. Create POST Endpoint with Validation
**File**: `UpdateTodo/Endpoint.cs`

```csharp
namespace March.Web.Features.Client.Todo.UpdateTodo;

public class Endpoint
{
    public const string Path = "/todo/update";
    private const string summary = "Update todo item";

    public Endpoint(RouteGroupBuilder route)
    {
        route
            .MapPost(Path, Handler)
            .Produces<RazorComponentResult<Components.Todo>>()
            .WithFeatureFlags(FeatureFlag.Todo)        // Optional
            .WithValidation<Validator>()                // Optional
            .AllowAnonymous()
            .WithSummary(summary);
    }

    public static IResult Handler(Request request, ValidationContext validationContext)
    {
        // Return component with updated data
        return new RazorComponentResult<Components.Todo>(new
        {
            Title = request.Title
        });
    }
}
```

**File**: `UpdateTodo/Request.cs`

```csharp
namespace March.Web.Features.Client.Todo.UpdateTodo;

public record Request(string Title);
```

**File**: `UpdateTodo/Validator.cs`

```csharp
namespace March.Web.Features.Client.Todo.UpdateTodo;

public class Validator : ValidatorBase<Validator, Request>
{
    public Validator(ILogger<Validator> logger, ValidationContext validationContext)
        : base(logger, validationContext)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(100)
            .WithMessage("Title must be 100 characters or less");
    }
}
```

**Key conventions**:
- Validators inherit from `ValidatorBase<TValidator, TRequest>`
- Constructor requires `ILogger<TValidator>` and `ValidationContext` (injected)
- Must call base constructor with both parameters
- Applied via `.WithValidation<Validator>()` on route

---

#### 5. Register in Program.cs
**Location**: `src/March.Web/Program.cs`

Add to the endpoint instantiation list:

```csharp
// Add client-side features
_ = new March.Web.Features.Client.Todo.GetTodo.Endpoint(routeGroup);
_ = new March.Web.Features.Client.Todo.UpdateTodo.Endpoint(routeGroup);
```

**Why This Works**:
- Endpoint constructors self-register when instantiated
- `routeGroup` is passed in and used for registration
- No need to manually call `MapGet`/`MapPost` in Program.cs

---

#### 6. Add Feature Flag (Optional)
**File**: `src/March.Web/Utils/Enums.cs`

```csharp
public enum FeatureFlag
{
    // ...existing flags
    Todo
}
```

**File**: `src/March.Web/appsettings.json`

```json
{
  "Features": {
    "Todo": {
      "Enabled": true
    }
  }
}
```

---

### Available Route Builder Extensions

Use these in the endpoint constructor when registering routes:

- `.WithFeatureFlags(FeatureFlag.{Name})` - Requires feature flag to be enabled
- `.WithValidation<TValidator>()` - Adds request validation via filter
- `.AllowAnonymous()` - Allows unauthenticated access
- `.RequireAuthorization()` - Requires authentication
- `.Produces<T>()` - OpenAPI documentation for response type
- `.WithSummary(string)` - Endpoint description for OpenAPI

---

## Common Patterns

### HTMX Usage Conventions

#### Pattern 1: Self-Replacing Component
Most common pattern - component replaces itself with updated state.

```html
<div id="counter">
    <button hx-post="counter/update"
            hx-target="#counter"
            hx-swap="outerHTML"
            hx-ext="json-enc"
            hx-vals='{"Count": @(Count + 1)}'>
        Increment
    </button>
</div>

@code {
    [Parameter] 
    public int Count { get; set; }
}
```

**Key attributes**:
- `hx-post` (or `hx-get`): Endpoint to call
- `hx-target`: CSS selector for element to update (usually the component's root id)
- `hx-swap="outerHTML"`: Replace the entire target element
- `hx-ext="json-enc"`: Encode body as JSON
- `hx-vals`: JSON object sent in request body (matches Request DTO properties)

#### Pattern 2: Validation Error Display
Access validation errors via injected `ValidationContext`:

```razor
@if (ValidationContext.TryGetError(nameof(Model.PropertyName), out var error))
{
    <p class="text-red-500">@error.ErrorMessage</p>
}
```

### Endpoint Return Patterns

All endpoints return `RazorComponentResult<T>` to render Razor components.

#### Returning a component without parameters
```csharp
public static IResult Handler()
{
    return new RazorComponentResult<Components.MyComponent>();
}
```

#### Returning a component with parameters
```csharp
public static IResult Handler(Request request)
{
    return new RazorComponentResult<Components.MyComponent>(new
    {
        Title = request.Title,
        Count = request.Count
    });
}
```

**Key points**:
- Anonymous object properties must match component `[Parameter]` names
- Use direct `new RazorComponentResult<T>()` instantiation
- Generic type references the component in `Components/` folder

### Service Injection in Razor Components
```razor
@code {
    [Inject] public ValidationContext ValidationContext { get; set; } = new();
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IHttpContextAccessor HttpContextAccessor { get; set; } = null!;
}
```

Always provide default values or null-forgiving operator.

---

## Feature Flags

### Configuration Structure
**File**: `src/March.Web/appsettings.json`

```json
{
  "Features": {
    "FeatureName": {
      "Enabled": true
    }
  }
}
```

### How Feature Flags Work
1. **Define enum** in `Utils/Enums.cs`:
   ```csharp
   public enum FeatureFlag { Counter, Form, Todo }
   ```

2. **Add to appsettings.json** with same name as enum value

3. **Apply to route** in `Program.cs`:
   ```csharp
   .WithFeatureFlags(FeatureFlag.FeatureName)
   ```

4. **Runtime behavior**:
   - Evaluated at the start of each request
   - If disabled, request pipeline short-circuits
   - Endpoint handler never executes
   - Can be toggled without recompiling

### Best Practices
- Use feature flags for:
  - Gradual feature rollouts
  - A/B testing
  - Environment-specific features
  - Quick feature disabling in production
- Don't use for:
  - Authorization (use `.RequireAuthorization()` instead)
  - Permanent features (remove flag after stable)

---

## Validation

### Validation Flow
1. Request arrives → Validator executes (if registered via `.WithValidation<T>()`)
2. Validation results stored in `ValidationContext` (scoped service)
3. Request continues to endpoint handler (regardless of validation result)
4. Endpoint returns component (can check validation state)
5. Component displays validation errors

### Creating Validators
Inherit from `ValidatorBase<TValidator, TRequest>` and inject dependencies:

```csharp
namespace March.Web.Features.Client.Form.SubmitForm;

public class Validator : ValidatorBase<Validator, Request>
{
    public Validator(ILogger<Validator> logger, ValidationContext validationContext)
        : base(logger, validationContext)
    {
        RuleFor(x => x.Property)
            .NotEmpty()
            .WithMessage("Custom error message");
    }
}
```

**Key requirements**:
- Inherit from `ValidatorBase<TValidator, TRequest>` (two generic parameters)
- Constructor must accept `ILogger<TValidator>` and `ValidationContext`
- Must call base constructor: `base(logger, validationContext)`
- Automatically registered via `AddValidatorsFromAssemblyContaining<Program>()` in `Program.cs`

### Registering Validators
Apply to endpoints via `.WithValidation<Validator>()`:

```csharp
public Endpoint(RouteGroupBuilder route)
{
    route
        .MapPost(Path, Handler)
        .WithValidation<Validator>()  // Registers validation filter
        .AllowAnonymous();
}
```

### Accessing Validation Results

**In Razor Components**:
```razor
@if (ValidationContext.TryGetError(nameof(Model.PropertyName), out var error))
{
    <p>@error.ErrorMessage</p>
}
```

**In Endpoints** (rare):
```csharp
public static IResult MyEndpoint(Request request, ValidationContext validationContext)
{
    if (!validationContext.IsValid)
    {
        // Handle invalid state
    }
}
```

---

## Static Files

### Web Root Configuration
**Location**: `src/March.Web/Features/Server/StaticFiles/`

This is configured as the web root in `Program.cs`:
```csharp
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "Features/Server/StaticFiles"
});
```

### File Access
Files in `Features/Server/StaticFiles/` are served directly:
- `index.html` → Served at `/` (entry point)
- `App.compiled.css` → Served at `/App.compiled.css`
- `App.js` → Served at `/App.js`
- `/img/photo.png` → Served at `/img/photo.png`

### Adding Static Files
1. Place files in `Features/Server/StaticFiles/`
2. Reference in HTML/Razor: `<img src="/img/myimage.png" />`
3. Files are automatically copied to output directory (configured in `.csproj`)

---

## Utilities & Helpers

### Available Utilities
Located in `src/March.Web/Utils/`:

- **Constants.cs**: HTTP method constants (`HTTP.GET`, `HTTP.POST`)
- **Enums.cs**: Feature flags and other enums
- **Extensions.cs**: Extension methods for route configuration and validation
- **ValidationContext.cs**: Scoped service for validation results
- **ValidatorBase.cs**: Base class for FluentValidation validators with filter integration
- **StringToBooleanConverter.cs**: JSON converter for string→bool

### Key Extension Methods

#### Validation Context Helper
```csharp
ValidationContext.TryGetError(nameof(PropertyName), out var error)
```
Used in Razor components to retrieve validation errors by property name.

#### Route Configuration Extensions
```csharp
.WithFeatureFlags(FeatureFlag.FeatureName)
.WithValidation<Validator>()
```

Applied in endpoint constructors to add filters to routes.

---

## Quick Reference

### Adding a New GET Endpoint

1. Create folder: `Features/Client/{Feature}/Get{Feature}/`
2. Add `Endpoint.cs`:
```csharp
namespace March.Web.Features.Client.{Feature}.Get{Feature};

public class Endpoint
{
    public const string Path = "/{feature}";
    private const string summary = "Get {feature}";

    public Endpoint(RouteGroupBuilder route)
    {
        route.MapGet(Path, Handler).Produces<RazorComponentResult<Components.{Feature}>>().AllowAnonymous();
    }

    public static IResult Handler()
    {
        return new RazorComponentResult<Components.{Feature}>();
    }
}
```
3. Register in `Program.cs`: `_ = new March.Web.Features.Client.{Feature}.Get{Feature}.Endpoint(routeGroup);`

### Adding a POST Endpoint with Validation

1. Create folder: `Features/Client/{Feature}/{Operation}/`
2. Add `Request.cs`: `public record Request(string Property);`
3. Add `Validator.cs`:
```csharp
public class Validator : ValidatorBase<Validator, Request>
{
    public Validator(ILogger<Validator> logger, ValidationContext validationContext)
        : base(logger, validationContext)
    {
        RuleFor(x => x.Property).NotEmpty();
    }
}
```
4. Add `Endpoint.cs` with `.WithValidation<Validator>()`
5. Register in `Program.cs`

### Component with HTMX Self-Update

```razor
<div id="my-component">
    <button hx-post="/my-component/update"
            hx-target="#my-component"
            hx-swap="outerHTML"
            hx-ext="json-enc"
            hx-vals='{"Value": @(Value + 1)}'>
        Update
    </button>
</div>

@code {
    [Parameter] public int Value { get; set; }
}
```

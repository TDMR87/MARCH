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
- **.NET 9.0 SDK** - Required for building and running the ASP.NET Core application
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
The project is organized by **features**, not layers. Each feature contains all its related code in one folder:
- Razor components (UI)
- Endpoint handlers (HTTP logic)
- Validators (validation rules)
- Models/DTOs (data structures)

### Folder Structure
```
src/March.Web/
├── Features/
│   ├── Client/           # User-facing features
│   │   ├── Counter/      # Example: Counter feature
│   │   │   ├── Counter.razor
│   │   │   ├── CounterEndpoint.cs
│   │   │   └── CounterValidator.cs
│   │   ├── Home/
│   │   ├── Auth/
│   │   └── ...
│   └── Server/           # Server features & static files
│       ├── StaticFiles/  # Web root (served as /)
│       └── ...
├── Utils/                # Shared utilities
├── Program.cs            # Entry point & route registration
├── App.razor             # Main layout component
└── _Imports.razor        # Global Razor imports
```

### Client vs Server Features
- **Client/**: Features that render UI and handle user interactions
- **Server/**: Infrastructure features (static files, middleware, etc.)

---

## Creating New Features

### 1. Create Feature Folder
Location: `src/March.Web/Features/Client/{FeatureName}/`

Example: Creating a "Todo" feature
```
Features/Client/Todo/
├── Todo.razor
├── TodoEndpoint.cs
└── TodoValidator.cs  (optional)
```

### 2. Create Razor Component
**File**: `Todo.razor`
```razor
<div id="todo">
    <!-- Your HTML with Tailwind classes -->
    <h1 class="text-2xl font-bold">@Model.Title</h1>
</div>

@code {
    [Parameter] public TodoModel Model { get; set; } = new();
    [Inject] public ValidationContext ValidationContext { get; set; } = new();
    
    public record TodoModel
    {
        public string Title { get; set; } = "";
        public string ToJson() => JsonSerializer.Serialize(this);
    }
}
```

**Key conventions**:
- Root element must have an `id` attribute (used for HTMX targeting)
- Use `[Parameter]` for component parameters
- Inject `ValidationContext` if validation is needed
- Include `ToJson()` method on models for HTMX `hx-vals` serialization

### 3. Create Endpoint Handler
**File**: `TodoEndpoint.cs`
```csharp
namespace March.Web.Features.Client.Todo;

public class TodoEndpoint
{
    public record Request(string Title);

    public static IResult GetTodo()
    {
        return Component<Todo>();
    }

    public static IResult UpdateTodo(Request request)
    {
        return Component<Todo, TodoModel>(model: new()
        {
            Title = request.Title
        });
    }
}
```

**Key conventions**:
- Static class with static methods
- Define request DTOs as nested records
- Use `Component<TRazorComponent>()` helper to return Razor components
- Second generic parameter passes model data to component

### 4. Create Validator (Optional)
**File**: `TodoValidator.cs`
```csharp
namespace March.Web.Features.Client.Todo;

public class TodoValidator : ValidatorBase<TodoEndpoint.Request>
{
    public TodoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
    }
}
```

Inherits from `ValidatorBase<T>` which implements FluentValidation.

### 5. Register Routes in Program.cs
**Location**: `src/March.Web/Program.cs`

```csharp
routes.AddRoutePath(HTTP.GET, "/todo", TodoEndpoint.GetTodo)
      .Produces<RazorComponentResult<Todo>>()
      .AllowAnonymous()  // or .RequireAuthorization()
      .WithSummary("Get todo component");

routes.AddRoutePath(HTTP.POST, "/todo/update", TodoEndpoint.UpdateTodo)
      .Produces<RazorComponentResult<Todo>>()
      .WithFeatureFlags(FeatureFlag.Todo)        // Optional
      .WithValidation<TodoValidator>()            // Optional
      .AllowAnonymous()
      .WithSummary("Update todo component");
```

**Available extensions**:
- `.WithFeatureFlags(FeatureFlag.{Name})` - Requires feature flag
- `.WithValidation<TValidator>()` - Adds request validation
- `.AllowAnonymous()` / `.RequireAuthorization()` - Auth requirements
- `.Produces<T>()` - OpenAPI documentation
- `.WithSummary(string)` - Endpoint description

### 6. Add Feature Flag (Optional)
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

## Common Patterns

### HTMX Usage Conventions

#### Pattern 1: Self-Replacing Component
Most common pattern - component replaces itself with updated state.

```html
<div id="component-name">
    <button hx-post="/component/update"
            hx-target="#component-name"
            hx-swap="outerHTML"
            hx-ext="json-enc"
            hx-vals='@((Model with { Value = newValue }).ToJson())'>
        Update
    </button>
</div>
```

**Key attributes**:
- `hx-post` (or `hx-get`): Endpoint to call
- `hx-target`: CSS selector for element to update (usually the component's root id)
- `hx-swap="outerHTML"`: Replace the entire target element
- `hx-ext="json-enc"`: Encode body as JSON
- `hx-vals`: JSON payload sent in request body

#### Pattern 2: Record "with" Syntax
Use C# record `with` expressions to create modified copies:

```csharp
@((Model with { Count = Model.Count + 1 }).ToJson())
```

This creates a new model instance with one property changed.

#### Pattern 3: Validation Error Display
Access validation errors via injected `ValidationContext`:

```razor
@if (ValidationContext.TryGetError(nameof(Model.PropertyName), out var error))
{
    <p class="text-red-500">@error.ErrorMessage</p>
}
```

### Endpoint Return Patterns

#### Returning a component without data
```csharp
public static IResult GetComponent()
{
    return Component<MyComponent>();
}
```

#### Returning a component with data
```csharp
public static IResult GetComponent(Request request)
{
    return Component<MyComponent, MyModel>(model: new()
    {
        Property = request.Value
    });
}
```

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
Inherit from `ValidatorBase<TRequest>`:

```csharp
public class MyValidator : ValidatorBase<MyEndpoint.Request>
{
    public MyValidator()
    {
        RuleFor(x => x.Property)
            .NotEmpty()
            .WithMessage("Custom error message");
    }
}
```

Automatically registered via `AddValidatorsFromAssemblyContaining<Program>()` in `Program.cs`.

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
- **Extensions.cs**: Extension methods (route registration helpers)
- **ValidationContext.cs**: Scoped service for validation results
- **ValidatorBase.cs**: Base class for FluentValidation validators
- **StringToBooleanConverter.cs**: JSON converter for string→bool

### Key Extension Methods

#### Route Registration
```csharp
routes.AddRoutePath(HTTP.GET, "/path", HandlerMethod)
```

#### Component Result Helper
```csharp
Component<TRazorComponent>()
Component<TRazorComponent, TModel>(model: modelInstance)
```

Both are static imports - available globally via `GlobalUsings.cs`.

using FormBuilder.Infrastructure.Data;
using FormBuilder.API.ExceptionHandlers;
using FormBuilder.API.Extensions;
using FormBuilder.API.HealthChecks;
using FormBuilder.Core.Models;
using FormBuilder.Core.Configuration;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using FormBuilder.API.Middleware;
using FormBuilder.API.Filters;
using FormBuilder.Core.IServices;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Localization
// -----------------------------
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// -----------------------------
// Validation Message Service
// -----------------------------
builder.Services.AddScoped<FormBuilder.Services.Services.Common.ValidationMessageService>();

// -----------------------------
// Controllers + JSON + ProblemDetails
// -----------------------------
builder.Services.AddControllers(options =>
    {
        // Add global validation filter
        options.Filters.Add<ValidationFilter>();
        // Suppress automatic model state validation for FluentValidation
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        // Add custom model validator provider to skip validation for optional fields
        options.ModelValidatorProviders.Add(new FormBuilder.API.Filters.CustomModelValidatorProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Suppress automatic model state validation - let our custom ValidationFilter handle it
        // This allows our filter to clean up ModelState errors before validation
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(FormBuilder.Services.Validators.FormBuilder.ApprovalStageAssigneesCreateDtoValidator).Assembly);

builder.Services.AddEndpointsApiExplorer();

// Add ProblemDetails service for exception handling
builder.Services.AddProblemDetails();

// -----------------------------
// Swagger Configuration (بسيط كما كان سابقاً)
// -----------------------------
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FormBuilder API",
        Version = "v1",
        Description = "API for FormBuilder Application"
    });

    // Use fully qualified type names for schema IDs to avoid conflicts
    c.CustomSchemaIds(type => 
    {
        if (type == null) return null;
        
        // Build a unique schema ID that includes full namespace and generic arguments
        string BuildSchemaId(Type t)
        {
            if (!t.IsGenericType)
            {
                return (t.FullName ?? t.Name)?.Replace("+", ".") ?? t.Name;
            }
            
            var genericType = t.GetGenericTypeDefinition();
            var genericTypeName = (genericType.FullName ?? genericType.Name)?.Split('`')[0] ?? genericType.Name;
            
            // Recursively process generic arguments to handle nested generics
            var args = t.GetGenericArguments()
                .Select(BuildSchemaId)
                .ToArray();
            
            // Use full namespace to ensure uniqueness
            var namespacePart = genericType.Namespace?.Replace(".", "_") ?? "";
            var typeNamePart = genericTypeName.Replace("+", ".");
            var argsPart = string.Join("_", args);
            
            return $"{namespacePart}_{typeNamePart}_{argsPart}";
        }
        
        var schemaId = BuildSchemaId(type);
        // Clean up special characters
        return schemaId.Replace("+", ".").Replace("`", "_").Replace("[", "").Replace("]", "").Replace(",", "");
    });

    // Ignore IActionResult and File results for Swagger schema generation
    c.MapType<IActionResult>(() => new OpenApiSchema { Type = "object" });
    c.MapType<FileResult>(() => new OpenApiSchema { Type = "string", Format = "binary" });
    c.MapType<FileStreamResult>(() => new OpenApiSchema { Type = "string", Format = "binary" });
    
    // Ignore obsolete items to avoid schema generation issues
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    
    // Suppress schema generation errors
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Add operation filter to handle file uploads
    c.OperationFilter<FileUploadOperationFilter>();
});

// -----------------------------
// Database Contexts
// -----------------------------

// Security / Auth DbContext
builder.Services.AddDbContextPool<AkhmanageItContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("AuthConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = "Server=DESKTOP-B3NJLJM;Database=AkhmanageItDb;Trusted_Connection=True;TrustServerCertificate=True;";
        Console.WriteLine("Using default Auth connection string");
    }

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(60); // 60 seconds command timeout
    });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

// Business / Forms DbContext
builder.Services.AddDbContextPool<FormBuilderDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = "Server=DESKTOP-B3NJLJM;Database=FormBuilderDb;Trusted_Connection=True;TrustServerCertificate=True;";
        Console.WriteLine("Using default FormBuilder connection string");
    }

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(60); // 60 seconds command timeout
    });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

// -----------------------------
// Caching
// -----------------------------
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// -----------------------------
// Data Protection (for encrypting stored secrets like SMTP password)
// Persist keys to filesystem so encrypted values remain decryptable after restart.
// -----------------------------
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")))
    .SetApplicationName("FormBuilder");

// -----------------------------
// HttpClient for External API Calls
// Configure named HttpClient with automatic decompression
// -----------------------------
// Register the default HttpClient factory explicitly so services that depend on
// IHttpClientFactory can always be activated.
builder.Services.AddHttpClient();

builder.Services.AddHttpClient("ExternalApi")
    .ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler
    {
        AutomaticDecompression = System.Net.DecompressionMethods.All // Enable automatic decompression for GZip, Deflate, and Brotli
    });

builder.Services.Configure<CrystalBridgeOptions>(
    builder.Configuration.GetSection(CrystalBridgeOptions.SectionName));

builder.Services.AddHttpClient("CrystalBridge", (sp, client) =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CrystalBridgeOptions>>().Value;
    if (string.IsNullOrWhiteSpace(options.BaseUrl))
        return;

    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds <= 0 ? 120 : options.TimeoutSeconds);
});

// -----------------------------
// Health Checks
// -----------------------------
builder.Services.AddHealthChecks()
    .AddCheck<FormBuilderDbHealthCheck>("formbuilder-db", tags: new[] { "db", "ready" })
    .AddCheck<AuthDbHealthCheck>("auth-db", tags: new[] { "db", "ready" });

// -----------------------------
// Response Compression
// -----------------------------
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

// -----------------------------
// Email Configuration
// -----------------------------
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.SectionName));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.SectionName));

// Override SMTP password from configuration or environment variable
// Priority: Environment Variable > Configuration["SMTP_PASSWORD"] > Configuration["Smtp:Password"]
var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD") 
    ?? builder.Configuration["SMTP_PASSWORD"] 
    ?? builder.Configuration["Smtp:Password"];
    
if (!string.IsNullOrWhiteSpace(smtpPassword))
{
    builder.Services.Configure<SmtpOptions>(options =>
    {
        options.Password = smtpPassword;
    });
}

// -----------------------------
// Dependency Injection
// -----------------------------

builder.Services.AddFormBuilderServices();


// Register GlobalExceptionHandler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// -----------------------------
// JWT Authentication
// -----------------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"];

if (string.IsNullOrEmpty(key))
{
    key = "99n6tDRTzftaPXYI8/ohgs0WsMWS1Yd9JuY=";
    Console.WriteLine("Warning: Using default JWT key");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "http://localhost:5000",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "FormBuilderClients",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    // Keep auth event logging only in development to avoid per-request console overhead in production.
    if (builder.Environment.IsDevelopment())
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    }
});

// -----------------------------
// Rate Limiting Configuration
// -----------------------------
builder.Services.Configure<RateLimitingOptions>(
    builder.Configuration.GetSection(RateLimitingOptions.SectionName));

// -----------------------------
// CORS - Improved Security
// -----------------------------
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:3000", "http://localhost:5173", "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    // Specific policy for Angular dev server
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Important for cookies/auth headers
    });

    // Keep AllowAll for development only
    // Note: AllowAnyOrigin cannot be used with AllowCredentials
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    }
});

// -----------------------------
// Build Application
// -----------------------------
var app = builder.Build();

// -----------------------------
// Middleware Pipeline + Localization
// -----------------------------

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ar")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    
    app.UseSwaggerUI(c =>
    {
        // Swagger endpoint for v1 (كما كان سابقاً)
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FormBuilder API v1");

        c.RoutePrefix = "swagger";
        c.DisplayRequestDuration();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
        c.EnableValidator();
    });
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}




app.UseHttpsRedirection();

// Response Compression
app.UseResponseCompression();

// Add Rate Limiting Middleware (قبل Routing)
app.UseRateLimiting();

app.UseRouting();

// CORS - Use Angular dev policy in development, specific origins in production
// Note: UseCors must be called after UseRouting and before UseAuthentication
app.UseCors(builder.Environment.IsDevelopment() ? "AllowAngularDev" : "AllowSpecificOrigins");

// Add exception handling middleware
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

// Error handling endpoint
app.Map("/error", ap => ap.Run(async context =>
{
    context.Response.StatusCode = 500;
    await context.Response.WriteAsync("An error occurred. Please contact administrator.");
}));



app.MapControllers();

// Health Check Endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                exception = e.Value.Exception?.Message,
                duration = e.Value.Duration.ToString()
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false // No checks for liveness, just returns 200 if app is running
});

// -----------------------------
// Database Seeding
// -----------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    try
    {
        var context = services.GetRequiredService<FormBuilderDbContext>();
        await DataSeeder.SeedAsync(context);
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database seeding completed successfully!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("FormBuilder API started successfully");
});

app.Run();

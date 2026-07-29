using KyrgyzTest.API;
using KyrgyzTest.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConnection(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введи JWT токен"
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
            Array.Empty<string>()
        }
    });
    
});
builder.Services.AddMemoryCache();
builder.Services.AddJwtAuth(builder.Configuration);

if (string.IsNullOrWhiteSpace(builder.Configuration["ExamSettings:AccessPassword"]))
    throw new InvalidOperationException(
        "ExamSettings:AccessPassword is not configured. Set the ExamSettings__AccessPassword environment variable.");


var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")
    ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    ?? builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? [];

bool IsPrivateLanOrLocalOrigin(string origin)
{
    if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;
    var host = uri.Host;
    if (host is "localhost" or "127.0.0.1") return true;
    if (!System.Net.IPAddress.TryParse(host, out var ip)) return false;
    var b = ip.GetAddressBytes();
    if (b.Length != 4) return false;
    return b[0] == 10
        || (b[0] == 172 && b[1] is >= 16 and <= 31)
        || (b[0] == 192 && b[1] == 168);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => allowedOrigins.Contains(origin) || IsPrivateLanOrLocalOrigin(origin))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<KyrgyzTest.Infrastructure.Persistence.AppDbContext>();
    
    int retries = 5;
    while (retries > 0)
    {
        try
        {
            context.Database.Migrate();
            Console.WriteLine("--- Миграции успешно применены! ---");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"--- База еще не готова, ждем... (Осталось попыток: {retries}) ---");
            await Task.Delay(5000);
            if (retries == 0) throw;
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

var uploadsPath = Path.Combine(AppContext.BaseDirectory, "uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(AppContext.BaseDirectory, "uploads")),
    RequestPath = "/uploads"
});
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

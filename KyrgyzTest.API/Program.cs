using KyrgyzTest.API;
using KyrgyzTest.API.Extensions;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
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
builder.Services.AddJwtAuth(builder.Configuration);


var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
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

            var existing = context.Users.FirstOrDefault(u => u.Login == "superadmin");
            if (existing != null)
                context.Users.Remove(existing);

            context.Users.Add(new Users
            {
                Id = Guid.NewGuid(),
                FullName = "Super Admin",
                Login = "superadmin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.SuperAdmin,
                CreatedAt = DateTime.UtcNow
            });
            context.SaveChanges();
            Console.WriteLine("--- SuperAdmin пересоздан ---");

            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"--- База еще не готова, ждем... (Осталось попыток: {retries}) ---");
            Thread.Sleep(5000); // Подождать 5 секунд перед следующей попыткой
            if (retries == 0) throw;
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

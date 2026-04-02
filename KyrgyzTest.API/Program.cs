using KyrgyzTest.API.Extensions;
using KyrgyzTest.API.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConnection(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();
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
            Thread.Sleep(5000); // Подождать 5 секунд перед следующей попыткой
            if (retries == 0) throw;
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapHub<StationHub>("/hub/station");
app.Run();

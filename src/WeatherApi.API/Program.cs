using Microsoft.EntityFrameworkCore;
using WeatherApi.Application.Services;
using WeatherApi.Domain.Repositories;
using WeatherApi.Infrastructure.Data;
using WeatherApi.Infrastructure.Repositories;
using WeatherApi.Infrastructure.Services;
using WeatherApi.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add memory cache
builder.Services.AddMemoryCache();

// Configure database
builder.Services.AddDbContext<WeatherApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();

// Register application services
builder.Services.AddScoped<IApiKeyValidationService, ApiKeyValidationService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

var app = builder.Build();

// Auto-migrate and seed database in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<WeatherApiDbContext>();
    
    try
    {
        await context.Database.MigrateAsync();
        await DataSeeder.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add API key middleware
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();

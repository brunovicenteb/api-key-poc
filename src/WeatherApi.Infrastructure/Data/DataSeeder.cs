using Microsoft.EntityFrameworkCore;
using WeatherApi.Domain.Entities;
using WeatherApi.Infrastructure.Data;

namespace WeatherApi.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedDataAsync(WeatherApiDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists
        if (await context.ApiKeys.AnyAsync())
        {
            return; // Data already seeded
        }

        // Seed sample API keys
        var apiKeys = new[]
        {
            new ApiKey
            {
                Id = Guid.NewGuid(),
                Key = "test-key-123",
                Name = "Test Key 1",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddYears(1),
                IsActive = true
            },
            new ApiKey
            {
                Id = Guid.NewGuid(),
                Key = "test-key-456",
                Name = "Test Key 2",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsActive = true
            },
            new ApiKey
            {
                Id = Guid.NewGuid(),
                Key = "expired-key-789",
                Name = "Expired Test Key",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                ExpiresAt = DateTime.UtcNow.AddDays(-1), // Already expired
                IsActive = true
            },
            new ApiKey
            {
                Id = Guid.NewGuid(),
                Key = "inactive-key-000",
                Name = "Inactive Test Key",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddYears(1),
                IsActive = false // Inactive
            }
        };

        context.ApiKeys.AddRange(apiKeys);
        await context.SaveChangesAsync();
    }
}
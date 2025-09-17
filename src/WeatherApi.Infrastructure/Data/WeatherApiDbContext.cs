using Microsoft.EntityFrameworkCore;
using WeatherApi.Domain.Entities;

namespace WeatherApi.Infrastructure.Data;

public class WeatherApiDbContext : DbContext
{
    public WeatherApiDbContext(DbContextOptions<WeatherApiDbContext> options) : base(options)
    {
    }

    public DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.ExpiresAt);
            entity.Property(e => e.IsActive).IsRequired();
            
            entity.HasIndex(e => e.Key).IsUnique();
        });
    }
}

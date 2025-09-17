using WeatherApi.Application.Services;
using WeatherApi.Domain.ValueObjects;

namespace WeatherApi.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    // Mock weather service - in real implementation, this would call external weather API
    private readonly Dictionary<string, (double Temperature, string Description)> _mockWeatherData = new()
    {
        { "london", (15.0, "Cloudy") },
        { "paris", (18.0, "Sunny") },
        { "new york", (12.0, "Rainy") },
        { "tokyo", (22.0, "Clear") },
        { "sydney", (25.0, "Partly Cloudy") },
        { "berlin", (8.0, "Overcast") },
        { "madrid", (20.0, "Sunny") },
        { "rome", (19.0, "Clear") }
    };

    public async Task<WeatherData?> GetWeatherAsync(string city, CancellationToken cancellationToken = default)
    {
        // Simulate API call delay
        await Task.Delay(100, cancellationToken);

        var normalizedCity = city.ToLowerInvariant().Trim();
        
        if (_mockWeatherData.TryGetValue(normalizedCity, out var weather))
        {
            return new WeatherData(
                city,
                weather.Temperature,
                weather.Description,
                DateTime.UtcNow
            );
        }

        // Return default weather for unknown cities
        return new WeatherData(
            city,
            16.0,
            "Weather data not available",
            DateTime.UtcNow
        );
    }
}
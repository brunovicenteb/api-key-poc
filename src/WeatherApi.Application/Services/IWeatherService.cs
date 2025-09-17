using WeatherApi.Domain.ValueObjects;

namespace WeatherApi.Application.Services;

public interface IWeatherService
{
    Task<WeatherData?> GetWeatherAsync(string city, CancellationToken cancellationToken = default);
}

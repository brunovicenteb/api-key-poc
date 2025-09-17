namespace WeatherApi.Application.Services;

public interface IApiKeyValidationService
{
    Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default);
}
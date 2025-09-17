using Microsoft.Extensions.Caching.Memory;
using WeatherApi.Domain.Repositories;

namespace WeatherApi.Application.Services;

public class ApiKeyValidationService : IApiKeyValidationService
{
    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

    public ApiKeyValidationService(IApiKeyRepository apiKeyRepository, IMemoryCache memoryCache)
    {
        _apiKeyRepository = apiKeyRepository;
        _memoryCache = memoryCache;
    }

    public async Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return false;

        var cacheKey = $"apikey_{apiKey}";
        
        // Check cache first
        if (_memoryCache.TryGetValue(cacheKey, out bool cachedResult))
        {
            return cachedResult;
        }

        // Check database
        var apiKeyEntity = await _apiKeyRepository.GetByKeyAsync(apiKey, cancellationToken);
        var isValid = apiKeyEntity?.IsValid ?? false;

        // Cache the result
        _memoryCache.Set(cacheKey, isValid, _cacheExpiration);

        return isValid;
    }
}
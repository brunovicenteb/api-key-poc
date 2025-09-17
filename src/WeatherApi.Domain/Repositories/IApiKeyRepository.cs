using WeatherApi.Domain.Entities;

namespace WeatherApi.Domain.Repositories;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<ApiKey?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApiKey>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiKey> CreateAsync(ApiKey apiKey, CancellationToken cancellationToken = default);
    Task<ApiKey> UpdateAsync(ApiKey apiKey, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
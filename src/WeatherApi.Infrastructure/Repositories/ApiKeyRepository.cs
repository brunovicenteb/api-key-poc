using Microsoft.EntityFrameworkCore;
using WeatherApi.Domain.Entities;
using WeatherApi.Domain.Repositories;
using WeatherApi.Infrastructure.Data;

namespace WeatherApi.Infrastructure.Repositories;

public class ApiKeyRepository : IApiKeyRepository
{
    private readonly WeatherApiDbContext _context;

    public ApiKeyRepository(WeatherApiDbContext context)
    {
        _context = context;
    }

    public async Task<ApiKey?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.ApiKeys
            .FirstOrDefaultAsync(ak => ak.Key == key, cancellationToken);
    }

    public async Task<ApiKey?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ApiKeys
            .FirstOrDefaultAsync(ak => ak.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ApiKey>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ApiKeys.ToListAsync(cancellationToken);
    }

    public async Task<ApiKey> CreateAsync(ApiKey apiKey, CancellationToken cancellationToken = default)
    {
        _context.ApiKeys.Add(apiKey);
        await _context.SaveChangesAsync(cancellationToken);
        return apiKey;
    }

    public async Task<ApiKey> UpdateAsync(ApiKey apiKey, CancellationToken cancellationToken = default)
    {
        _context.ApiKeys.Update(apiKey);
        await _context.SaveChangesAsync(cancellationToken);
        return apiKey;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var apiKey = await GetByIdAsync(id, cancellationToken);
        if (apiKey != null)
        {
            _context.ApiKeys.Remove(apiKey);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
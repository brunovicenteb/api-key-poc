using WeatherApi.Application.Services;

namespace WeatherApi.API.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IApiKeyValidationService _apiKeyValidationService;
    private readonly ILogger<ApiKeyMiddleware> _logger;

    public ApiKeyMiddleware(
        RequestDelegate next, 
        IApiKeyValidationService apiKeyValidationService,
        ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _apiKeyValidationService = apiKeyValidationService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip API key validation for non-API endpoints
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        // Get API key from header
        if (!context.Request.Headers.TryGetValue("x-api-key", out var extractedApiKey))
        {
            _logger.LogWarning("API key is missing from request to {Path}", context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is required");
            return;
        }

        var apiKey = extractedApiKey.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogWarning("Empty API key provided for request to {Path}", context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is required");
            return;
        }

        // Validate API key
        var isValid = await _apiKeyValidationService.ValidateApiKeyAsync(apiKey);
        if (!isValid)
        {
            _logger.LogWarning("Invalid API key provided: {ApiKey}", apiKey);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API key");
            return;
        }

        _logger.LogInformation("Valid API key provided for request to {Path}", context.Request.Path);
        await _next(context);
    }
}
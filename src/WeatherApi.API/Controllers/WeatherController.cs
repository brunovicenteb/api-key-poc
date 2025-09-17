using Microsoft.AspNetCore.Mvc;
using WeatherApi.Application.DTOs;
using WeatherApi.Application.Services;

namespace WeatherApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    /// <summary>
    /// Gets weather information for a specified city
    /// </summary>
    /// <param name="city">The city name to get weather for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Weather information for the specified city</returns>
    [HttpGet]
    public async Task<ActionResult<WeatherResponse>> GetWeather(
        [FromHeader(Name = "x-api-key")] string apiKey, 
        [FromQuery] string city, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return BadRequest("City parameter is required");
        }

        var weatherData = await _weatherService.GetWeatherAsync(city, cancellationToken);

        if (weatherData == null)
        {
            return NotFound($"Weather data not found for city: {city}");
        }

        var response = new WeatherResponse
        {
            City = weatherData.City,
            Temperature = weatherData.Temperature,
            Description = weatherData.Description,
            Timestamp = weatherData.Timestamp
        };

        return Ok(response);
    }
}
namespace WeatherApi.Application.DTOs;

public class WeatherResponse
{
    public string City { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
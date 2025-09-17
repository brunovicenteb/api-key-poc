namespace WeatherApi.Domain.ValueObjects;

public record WeatherData(
    string City,
    double Temperature,
    string Description,
    DateTime Timestamp
);
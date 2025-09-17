# Weather API with API Key Authentication

A .NET 8 REST API that provides weather information for cities using Clean Architecture. The API requires authentication via API keys stored in PostgreSQL with memory caching for performance.

## Architecture

This project follows Clean Architecture principles with the following layers:

- **Domain**: Contains entities, value objects, and interfaces
- **Application**: Contains business logic, services, and DTOs
- **Infrastructure**: Contains data access, external services, and infrastructure concerns
- **API**: Contains controllers, middleware, and API configuration

## Features

- 🌤️ Weather information by city name
- 🔐 API key authentication with x-api-key header
- 🗄️ PostgreSQL database for API key storage
- ⚡ Memory caching for API key validation
- ⏰ API key expiration support
- 🏗️ Clean Architecture implementation
- 📝 Swagger/OpenAPI documentation
- 🐳 Docker Compose for PostgreSQL

## Prerequisites

- .NET 8 SDK
- Docker and Docker Compose (for PostgreSQL)
- PostgreSQL (if not using Docker)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd api-key-poc
```

### 2. Start PostgreSQL with Docker Compose

```bash
docker-compose up -d
```

This will start a PostgreSQL container with:
- Database: `WeatherApiDb`
- Username: `postgres`
- Password: `postgres`
- Port: `5432`

### 3. Build and run the application

```bash
dotnet build
cd src/WeatherApi.API
dotnet run
```

The API will be available at `https://localhost:7123` (or check console output for actual port).

### 4. Access Swagger UI

Open your browser and navigate to `https://localhost:7123/swagger` to view the API documentation.

## API Usage

### Get Weather Information

**Endpoint:** `GET /api/weather`

**Headers:**
- `x-api-key`: Your API key (required)

**Query Parameters:**
- `city`: The city name (required)

**Example:**
```bash
curl -X GET "https://localhost:7123/api/weather?city=London" \
     -H "x-api-key: test-key-123"
```

**Response:**
```json
{
  "city": "London",
  "temperature": 15.0,
  "description": "Cloudy",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

## Sample API Keys

The application automatically seeds the database with sample API keys in development:

| API Key | Status | Expiration | Description |
|---------|--------|------------|-------------|
| `test-key-123` | Active | 1 year | Valid test key |
| `test-key-456` | Active | 30 days | Valid test key |
| `expired-key-789` | Expired | Yesterday | Expired test key |
| `inactive-key-000` | Inactive | 1 year | Inactive test key |

## Database Schema

### ApiKeys Table

| Column | Type | Description |
|--------|------|-------------|
| Id | UUID | Primary key |
| Key | VARCHAR(100) | Unique API key string |
| Name | VARCHAR(200) | Display name for the key |
| CreatedAt | TIMESTAMP | Creation timestamp |
| ExpiresAt | TIMESTAMP | Expiration timestamp (nullable) |
| IsActive | BOOLEAN | Whether the key is active |

## Configuration

### Database Connection

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=WeatherApiDb;Username=postgres;Password=postgres"
  }
}
```

### Memory Cache

API keys are cached in memory for 5 minutes by default. This can be configured in the `ApiKeyValidationService`.

## Development

### Adding New Migrations

```bash
dotnet ef migrations add <MigrationName> --project src/WeatherApi.Infrastructure --startup-project src/WeatherApi.API
```

### Updating Database

```bash
dotnet ef database update --project src/WeatherApi.Infrastructure --startup-project src/WeatherApi.API
```

## Testing

### Manual Testing

1. Start the application
2. Use the provided test API keys
3. Make requests to `/api/weather?city=<cityname>` with the `x-api-key` header

### Supported Cities

The mock weather service supports the following cities:
- London
- Paris
- New York
- Tokyo
- Sydney
- Berlin
- Madrid
- Rome

For unknown cities, it returns a default response.

## Error Responses

### 401 Unauthorized
- Missing `x-api-key` header
- Invalid API key
- Expired API key
- Inactive API key

### 400 Bad Request
- Missing `city` parameter

### 404 Not Found
- Weather data not available for the specified city

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
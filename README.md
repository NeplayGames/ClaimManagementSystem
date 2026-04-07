# Claims Management System

Claims Management System is a .NET 6 Web API for managing insurance claims through secure REST endpoints. It demonstrates backend development skills in ASP.NET Core, Entity Framework Core, JWT authorization, Swagger documentation, health checks, and GitHub Actions CI.

This project was built to show how a production style claims API can support claim creation, updates, status tracking, pagination, validation, and secure access control in a clean and maintainable backend structure.

## Overview

The API simulates a claims processing backend for an insurance platform. It allows authorized users to create claims, retrieve claim details, update records, manage claim status, and delete claims through secure endpoints.

The project focuses on backend design patterns commonly used in enterprise .NET applications, including layered structure, request validation, centralized exception handling, authorization, and CI automation.

## Key Features

- JWT authorization and secure endpoint access
- Claims CRUD endpoints
- Pagination support for claim listing
- Swagger UI for local API exploration
- Health and readiness endpoints
- Entity Framework Core based persistence
- Centralized middleware and exception handling
- GitHub Actions CI workflow
- Clean service and contract based structure

## Tech Stack

- C#
- .NET 6
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server or in memory data store
- JWT Authorization
- Swagger / OpenAPI
- GitHub Actions

## Prerequisites

- [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server or LocalDB if not using the in memory development store

## Configuration

The API reads settings from:

- `appsettings.json`
- `appsettings.Development.json`

Important settings:

- `DataStore:UseInMemoryForDevelopment`
  - `true` in development to use in memory DB
  - `false` to use SQL Server via `ConnectionStrings:DefaultConnection`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:SigningKey`

## Setup

Run the following commands to start the project locally:

```bash
dotnet restore
dotnet build --no-restore
dotnet run --project ClaimsManagementSystem.csproj
```

## Local Development URLs

By default, the application runs on:

- `https://localhost:7283`
- `http://localhost:5036`

If you are running in the Development environment, Swagger is available at:

- `https://localhost:7283/swagger`

The API also exposes these health endpoints:

- `GET /health`
- `GET /health/ready`

## Architecture

The application follows a clean backend structure with clear separation of concerns:

- `Controllers` handle HTTP requests and responses
- `Services/Claims` contains claims business logic
- `Contracts` defines request and response models
- `Data` manages persistence and database access
- `Middleware` handles centralized exception processing
- `Exceptions` contains custom exception types
- `Program.cs` configures services, middleware, authorization, Swagger, and health checks

## Build

Use the following command to build the project:

```bash
dotnet build --no-restore
```

## API Usage Examples

Use the following base values in the examples below:

```bash
BASE_URL="https://localhost:7283"
TOKEN="<your-jwt-token>"
```

### List claims

```bash
curl -X GET "$BASE_URL/api/claims?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Accept: application/json"
```

### Get a claim by ID

```bash
CLAIM_ID="11111111-1111-1111-1111-111111111111"

curl -X GET "$BASE_URL/api/claims/$CLAIM_ID" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Accept: application/json"
```

### Create a claim

```bash
curl -X POST "$BASE_URL/api/claims" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "Jane Doe",
    "description": "Rear bumper damage from parking lot collision",
    "incidentDate": "2026-03-20T00:00:00Z",
    "priority": "High"
  }'
```

### Update a claim

```bash
curl -X PUT "$BASE_URL/api/claims/$CLAIM_ID" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "description": "Updated estimate received from repair shop",
    "priority": "Medium",
    "status": "InReview"
  }'
```

### Delete a claim

```bash
curl -X DELETE "$BASE_URL/api/claims/$CLAIM_ID" \
  -H "Authorization: Bearer $TOKEN"
```

## Postman

You can import any of the cURL commands above directly into Postman:

1. Open Postman
2. Click **Import**
3. Choose **Raw text**
4. Paste one of the cURL commands
5. Click **Continue** and then **Import**

Suggested collection variables:

- `baseUrl` = `https://localhost:7283`
- `token` = your JWT token
- `claimId` = target claim GUID

You can then use requests such as:

- `GET {{baseUrl}}/api/claims`
- `GET {{baseUrl}}/api/claims/{{claimId}}`
- `POST {{baseUrl}}/api/claims`
- `PUT {{baseUrl}}/api/claims/{{claimId}}`
- `DELETE {{baseUrl}}/api/claims/{{claimId}}`

Include this header in protected requests:

- `Authorization: Bearer {{token}}`

## CI

The GitHub Actions workflow is available at `.github/workflows/ci.yml` and runs the following steps:

1. `dotnet restore`
2. `dotnet build --no-restore`

## Future Improvements

Possible next enhancements include:

- Add xUnit unit and integration tests
- Add Docker support for easier local setup
- Add seed data for demo usage
- Add audit logging
- Add API versioning
- Add more advanced filtering and sorting
- Add a small admin dashboard
- Add Azure deployment support

## Author

**Shreejan Pandey**

- Email: shreejanp.pandey@gmail.com
- Phone: 4696479438
- GitHub: [NeplayGames](https://github.com/NeplayGames)

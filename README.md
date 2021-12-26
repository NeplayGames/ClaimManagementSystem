# Claims Management System

A .NET 6 Web API for managing insurance claims with JWT-based authorization, EF Core persistence, and Swagger for local API exploration.

## Prerequisites

- [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- (Optional) SQL Server or LocalDB if not using the in-memory development store

## Configuration

The API reads settings from:

- `appsettings.json`
- `appsettings.Development.json`

Important settings:

- `DataStore:UseInMemoryForDevelopment`
  - `true` in development to use in-memory DB
  - `false` to use SQL Server via `ConnectionStrings:DefaultConnection`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:SigningKey`

## Setup

```bash
dotnet restore
```

## Run

```bash
dotnet run --project ClaimsManagementSystem.csproj
```

Default local URLs (from `Properties/launchSettings.json`):

- `https://localhost:7283`
- `http://localhost:5036`

Swagger UI (Development environment):

- `https://localhost:7283/swagger`

Health endpoints:

- `GET /health`
- `GET /health/ready`

## Build

```bash
dotnet build --no-restore
```

## Test

```bash
dotnet test --no-build
```

> This repository currently contains the API project only; if no test project is present, `dotnet test` will complete with 0 tests.

---

## API Usage Examples

Base URL used below:

```bash
BASE_URL="https://localhost:7283"
TOKEN="<your-jwt-token>"
```

### 1) List claims

```bash
curl -X GET "$BASE_URL/api/claims?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Accept: application/json"
```

### 2) Get claim by ID

```bash
CLAIM_ID="11111111-1111-1111-1111-111111111111"

curl -X GET "$BASE_URL/api/claims/$CLAIM_ID" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Accept: application/json"
```

### 3) Create claim

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

### 4) Update claim

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

### 5) Delete claim (Admin only)

```bash
curl -X DELETE "$BASE_URL/api/claims/$CLAIM_ID" \
  -H "Authorization: Bearer $TOKEN"
```

---

## Postman Examples

### Import as raw cURL

1. Open Postman.
2. Click **Import**.
3. Choose **Raw text**.
4. Paste one of the cURL commands above.
5. Click **Continue** → **Import**.

### Suggested Postman collection variables

- `baseUrl` = `https://localhost:7283`
- `token` = your JWT token
- `claimId` = target claim GUID

Then use requests such as:

- `GET {{baseUrl}}/api/claims`
- `GET {{baseUrl}}/api/claims/{{claimId}}`
- `POST {{baseUrl}}/api/claims`
- `PUT {{baseUrl}}/api/claims/{{claimId}}`
- `DELETE {{baseUrl}}/api/claims/{{claimId}}`

With header:

- `Authorization: Bearer {{token}}`

## CI

GitHub Actions workflow is available at `.github/workflows/ci.yml` and runs:

1. `dotnet restore`
2. `dotnet build --no-restore`
3. `dotnet test --no-build`

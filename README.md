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

Restore dependencies:

```bash
dotnet restore

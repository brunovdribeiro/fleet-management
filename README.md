# Fleet Management API

This project is a .NET 8 starter kit for a fleet management SaaS platform, built with Clean Architecture principles.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)

## How to Run

1. **Start the database and API:**

   ```bash
   docker-compose up -d --build
   ```

2. **Apply database migrations:**

   Once the container is running, apply the EF Core migrations to create the database schema.

   ```bash
   dotnet ef database update --startup-project src/Api --project src/Infrastructure
   ```

3. **Access the API:**

   The API will be available at `http://localhost:8080`. You can access the root endpoint to verify it's running.

## Project Structure

The solution follows the principles of Clean Architecture, with the following projects:

- `Domain`: Contains the core business entities and logic.
- `Application`: Contains the application logic, such as use cases and interfaces.
- `Infrastructure`: Contains the implementation of the interfaces defined in the `Application` layer, such as database access and external services.
- `Api`: The presentation layer, an ASP.NET Core Web API.
- `Workers`: For background processing tasks.
- `Tests`: For unit and integration tests.

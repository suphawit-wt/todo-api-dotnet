# Todo API .NET

This project implements a Todo API using .NET with SQL Server, Entity Framework for ORM, EF Core Tools for migrations, and JWT for authentication.

## Table of Contents

- [Development](#development)
	- [Configuration](#configuration)
    - [Setup](#setup)
    - [Testing](#testing)
    - [Build and Linting](#build-and-linting)
    - [URL](#url)
- [Deployment](#deployment)
	- [Build Production Image](#build-production-image)
    - [Run Docker Container](#run-docker-container)
- [License](#license)


## Development

### Configuration

Copy the `.env.example` file to a new file named `.env`, and then set the environment configuration in the `.env` file.
```bash
cp .env.example .env
```

### Setup
1. Using Docker Compose to initialize the project:
```bash
docker compose up -d
```

2. Start a terminal session in the app container
```bash
docker compose exec app bash
```

3. Install dependencies for development
```bash
dotnet restore --locked-mode
```

4. Run migrations
```bash
dotnet ef database update --project src/TodoApp.Api
```

5. Start the server for development
```bash
dotnet run --project src/TodoApp.Api
```

### Testing
Run the test suite using:
```bash
dotnet test
```

### Build and Linting
Check code quality with:
```bash
dotnet build --warnaserror
```

### URL
- **Swagger:** [http://localhost:5000/swagger](http://localhost:5000/swagger)

## Deployment

### Build Production Image
```bash
docker build -t todo-api-dotnet:1.0.0 .
```

### Run Docker Container
```bash
docker run --name todo-api-dotnet -p 5000:8080 --env-file .env -d todo-api-dotnet:1.0.0
```

## License

Distributed under the MIT License. See [LICENSE](../LICENSE) for more information.
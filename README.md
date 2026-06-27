# Welcome to JOI Delivery

JOI Delivery is a .NET API for adding grocery products to a user's cart and viewing the current cart. This refactored version preserves the original API behavior while organizing the codebase around Clean Architecture and testable business rules.

## Architecture Overview

The solution is split into explicit layers:

- `JOI.Delivery.Domain`: enterprise business objects and rules. It has no project dependencies.
- `JOI.Delivery.Application`: use cases, DTOs, service interfaces, validation, and orchestration. It depends only on Domain.
- `JOI.Delivery.Infrastructure`: in-memory repositories and seed data. It implements Application abstractions.
- `JoiDelivery`: ASP.NET Core presentation layer with controllers, startup, middleware, and dependency injection composition.
- `tests`: xUnit test projects for Domain and Application behavior.

## Solution Structure

```text
src
  JOI.Delivery.Domain
  JOI.Delivery.Application
  JOI.Delivery.Infrastructure
  JoiDelivery
tests
  JOI.Delivery.Domain.Tests
  JOI.Delivery.Application.Tests
JOI.Delivery.sln
```

## Dependency Flow

```text
JoiDelivery (Presentation)
  -> JOI.Delivery.Application
  -> JOI.Delivery.Domain

JOI.Delivery.Infrastructure
  -> JOI.Delivery.Application
  -> JOI.Delivery.Domain
```

The Domain project references no other project. Presentation does not depend directly on Domain persistence details, and Infrastructure is hidden behind interfaces.

## API

### Add Product to Cart

```http
POST /cart/product
Content-Type: application/json
```

```json
{
  "userId": "user101",
  "productId": "product101",
  "outletId": "store101"
}
```

### View Cart

```http
GET /cart/view?userId=user101
```

## Build and Run

```bash
dotnet restore
dotnet build
dotnet run --project src/JoiDelivery/JoiDelivery.csproj
```

The HTTP profile runs on `http://localhost:8080`.

## Test

```bash
dotnet test
```

Tests use xUnit, FluentAssertions, and Moq. The Application tests mock external dependencies so business workflows can be tested without running the API, touching the file system, or using seeded infrastructure.

## Architectural Improvements

- Moved business behavior out of controllers and into Application services and Domain entities.
- Replaced direct static seed-data access in business services with repository interfaces.
- Centralized dependency injection in the Presentation composition root and Infrastructure extension method.
- Added explicit validation and meaningful application exceptions.
- Added API exception middleware that maps validation and not-found failures to clear HTTP problem responses.
- Preserved route names, DTO names, model names, and service method names where practical.

## SOLID and Code Smell Improvements

- Single Responsibility: controllers handle HTTP only; services orchestrate use cases; repositories own data access.
- Open/Closed: new persistence implementations can be added without changing Application services.
- Interface Segregation: small repository and service interfaces expose only required operations.
- Dependency Inversion: Application depends on abstractions, while Infrastructure provides implementations.
- Removed tight coupling to static seed data from core workflows.
- Reduced hidden dependencies and replaced ad hoc object creation with constructor injection.
- Moved cart mutation into the `Cart` entity so the rule is unit testable.

## TDD Readiness

Future work can follow Red -> Green -> Refactor because business logic is isolated behind small classes with constructor-injected dependencies. Domain behavior can be tested directly, and Application workflows can be tested with mocks. Infrastructure can be swapped for database-backed repositories later without rewriting use cases.


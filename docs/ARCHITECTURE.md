# OrderFlow Architecture

OrderFlow is an order-management REST API. It uses the **Clean Architecture** / "Onion" layout,
which is the most common structure in enterprise .NET.

```
                 ┌──────────────────────────────┐
  HTTP  ───────► │  OrderFlow.Api               │  Controllers, middleware, Program.cs (composition root)
                 └──────────────┬───────────────┘
                                │ calls
                 ┌──────────────▼───────────────┐
                 │  OrderFlow.Application       │  Services (use cases), DTOs, repository INTERFACES
                 └──────────────┬───────────────┘
                                │ uses
                 ┌──────────────▼───────────────┐
                 │  OrderFlow.Domain            │  Entities, enums, business rules. No dependencies!
                 └──────────────▲───────────────┘
                                │ implements Application's interfaces
                 ┌──────────────┴───────────────┐
                 │  OrderFlow.Infrastructure    │  EF Core DbContext, repositories, external clients
                 └──────────────────────────────┘
```

**The dependency rule:** arrows point *inward*. Domain knows nothing about EF Core or HTTP.
Infrastructure depends on Application (to implement its interfaces), never the other way round.
`Program.cs` wires everything together via **dependency injection**.

## A request's journey: `GET /api/orders/1`
1. **Kestrel** (the web server) receives the request
2. **Middleware pipeline** in `Program.cs` runs in order: exception handler → HTTPS redirect → authorization → routing
3. **Routing** matches `[Route("api/orders")]` + `[HttpGet("{id:int}")]` → `OrdersController.GetById`
4. **Model binding** turns `1` from the URL into `int id`
5. Controller calls `IOrderService.GetByIdAsync` (DI gives it an `OrderService`)
6. Service calls `IOrderRepository.GetByIdAsync` (DI gives it the EF `OrderRepository`)
7. EF Core translates LINQ to SQL and runs it against SQLite
8. Entity → **DTO** mapping (never return EF entities directly from an API. Why? Over-posting, circular refs, leaking schema)
9. `Ok(dto)` → serialized to JSON by `System.Text.Json`
10. If anything throws, `GlobalExceptionHandler` converts it to a **ProblemDetails** response

## Key files
| File | Why it matters |
|------|----------------|
| `src/OrderFlow.Api/Program.cs` | Startup: DI registrations + middleware pipeline. **Read this first.** |
| `src/OrderFlow.Api/Middleware/GlobalExceptionHandler.cs` | Exception → HTTP status mapping |
| `src/OrderFlow.Application/Services/OrderService.cs` | The richest business logic |
| `src/OrderFlow.Domain/Entities/Order.cs` | Domain rules (and a couple of bugs) |
| `src/OrderFlow.Infrastructure/Persistence/AppDbContext.cs` | DB schema config (Fluent API) |
| `src/OrderFlow.Infrastructure/DependencyInjection.cs` | How Infrastructure plugs in |

## Service lifetimes (you WILL be asked about this)
| Lifetime | One instance per… | Use for |
|----------|-------------------|---------|
| `AddTransient` | every injection | lightweight, stateless helpers |
| `AddScoped` | HTTP request | DbContext, repositories, services that use them |
| `AddSingleton` | app lifetime | config, caches, `HttpClient` factories. Must be thread-safe! |

⚠️ **Captive dependency**: injecting a Scoped service into a Singleton keeps it alive forever (e.g. a DbContext shared across threads, which breaks). The DI container throws on this in Development.

## Things a real codebase would also have
MediatR/CQRS handlers, AutoMapper or Mapster, FluentValidation, Serilog → Seq/Datadog/App Insights,
Polly for retries, Docker + Kubernetes/Azure App Service, Azure Service Bus/RabbitMQ for events,
background jobs (Hangfire / `BackgroundService`), and integration tests with Testcontainers.
See `docs/GLOSSARY.md`.

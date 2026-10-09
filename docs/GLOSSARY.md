# .NET Backend Glossary: words you'll hear in week one

## C# / language
| Term | Meaning |
|------|---------|
| `async` / `await` / `Task<T>` | Non-blocking I/O. Rule: **async all the way down**. Never `.Result` / `.Wait()` (see OF-114). |
| `CancellationToken` | Lets a cancelled HTTP request stop DB work. Pass it through every async call. |
| `record` | Immutable type with value equality. The go-to for DTOs. |
| Nullable reference types | `string?` may be null, `string` shouldn't be. The `!` operator *only silences the compiler* (see OF-106). |
| LINQ | `Where/Select/OrderBy/Any/FirstOrDefault`. On `IQueryable` it becomes SQL, on `IEnumerable`/`List` it runs in memory. |
| `IQueryable` vs `IEnumerable` | Big perf difference: filter before you materialize (`ToList`). |
| Primary constructor | `class Foo(IBar bar)`: C# 12 syntax, used for DI throughout this repo. |
| Extension method | `static` method with `this` param, e.g. `services.AddInfrastructure()`. |
| `using` / `IDisposable` | Deterministic cleanup of resources. |
| Pattern matching / switch expression | `x switch { A => ..., _ => ... }` (see `GlobalExceptionHandler`). |

## ASP.NET Core
| Term | Meaning |
|------|---------|
| Kestrel | The built-in web server. |
| Middleware / pipeline | Chain of components each request flows through. **Order matters.** |
| Controller vs Minimal API | Class-based `[ApiController]` vs `app.MapGet(...)` lambdas. You'll see both. |
| Model binding | Maps route/query/body/header → method parameters (`[FromQuery]`, `[FromBody]`, `[FromRoute]`). |
| DI container | `builder.Services.Add…`; constructor injection everywhere. |
| Options pattern | `IOptions<T>` binds config sections (appsettings.json) to classes. |
| `appsettings.{Environment}.json` | Config layered by `ASPNETCORE_ENVIRONMENT` (Development/Staging/Production). |
| User secrets | `dotnet user-secrets`: dev-only secrets outside the repo. |
| ProblemDetails | Standard JSON error format (RFC 7807). |
| Filters | `ActionFilter`, `ExceptionFilter`: MVC-specific hooks, older alternative to middleware. |
| `IHostedService` / `BackgroundService` | Long-running background work inside the app. |
| `IHttpClientFactory` | The right way to create `HttpClient`s (avoids socket exhaustion). |

## Data
| Term | Meaning |
|------|---------|
| EF Core | Microsoft's ORM. `DbContext` = unit of work, `DbSet<T>` = table. |
| Change tracking | EF watches loaded entities and saves changes on `SaveChanges`. `AsNoTracking()` for read-only queries. |
| `Include` / eager loading | Load related data in the same query (OF-103). |
| N+1 problem | 1 query for the list + 1 per row. Watch the SQL log. |
| Migration | Versioned C# schema change (`dotnet ef migrations add`). |
| Dapper | Lightweight micro-ORM: you write SQL, it maps results. Common alongside EF for hot paths. |
| Stored procedure | SQL code in the DB. Very common in older/enterprise SQL Server shops. |
| Optimistic concurrency | `RowVersion` column; save fails if someone changed the row since you read it. |
| Transaction | All-or-nothing. One `SaveChanges` is already a transaction. |
| SQL Server / SSMS / Azure SQL | The DB you'll most likely use at a .NET job, and its GUI tool. |

## Architecture & patterns
| Term | Meaning |
|------|---------|
| Clean / Onion / N-tier | Layered architecture (see ARCHITECTURE.md). |
| Repository + Unit of Work | Abstraction over data access. (Debate: EF *already is* both.) |
| CQRS / MediatR | Separate Commands (writes) & Queries (reads). Each request → one handler class. **Very common.** |
| DTO | Data Transfer Object: the API contract, decoupled from the DB entity. |
| AutoMapper / Mapster | Entity↔DTO mapping libraries (or hand-written mapping, like this repo). |
| FluentValidation | Popular validation library. |
| DDD | Domain-Driven Design: aggregates, value objects, domain events, rich domain models. |
| Microservices / monolith / modular monolith | Deployment architecture styles. |
| Message bus | Azure Service Bus, RabbitMQ, Kafka. Async communication between services. MassTransit/NServiceBus wrap them. |
| Outbox pattern | Save DB change + outgoing message atomically. |
| Idempotency | Same request twice = same effect once (important for retries and payments). |
| Polly / resilience | Retries, circuit breakers, timeouts for outbound calls. |

## Testing
| Term | Meaning |
|------|---------|
| xUnit / NUnit / MSTest | Test frameworks (xUnit is most common now). `[Fact]` = one test, `[Theory]` = parameterized. |
| Moq / NSubstitute / FakeItEasy | Mocking libraries. |
| FluentAssertions / Shouldly | Readable assertions: `result.Should().Be(5)`. |
| `WebApplicationFactory` | In-process integration tests of the whole API. |
| Testcontainers | Spin up a real SQL Server/Postgres in Docker for tests. |
| AAA | Arrange, Act, Assert. |

## Ops / team life
| Term | Meaning |
|------|---------|
| CI/CD | Azure DevOps Pipelines or GitHub Actions: build → test → deploy on every PR/merge. |
| Azure App Service / AKS / Functions | Common .NET hosting targets. |
| Application Insights / Datadog / Seq / Serilog | Logging & monitoring. |
| Structured logging | `LogInformation("Order {OrderId}", id)`, not string interpolation! The placeholders become searchable fields. |
| Feature flag | Ship code dark, turn on later (LaunchDarkly, Azure App Configuration). |
| Standup / sprint / retro / refinement / planning | Scrum ceremonies. |
| DoD / AC | Definition of Done / Acceptance Criteria. |
| PR / code review / "LGTM" / "nit" | Pull request process. |
| On-call / PagerDuty / postmortem / RCA | Incident life. |
| Hotfix | Urgent fix straight to production, outside the normal release. |

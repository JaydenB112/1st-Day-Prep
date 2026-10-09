# 🚀 1st-Day-Prep: OrderFlow API

A **realistic practice codebase** for a first day as a .NET C# backend engineer.
It's a small order-management REST API built the way enterprise .NET teams build things, with a sprint board of tickets,
**intentional bugs**, an incident, a spike, and a code review exercise.

## Quick start
```bash
dotnet build
dotnet test                                                # 5 pass, 2 skipped (those are tickets!)
dotnet run --project src/OrderFlow.Api --launch-profile http
curl http://localhost:5287/api/version
```
Then open [`src/OrderFlow.Api/OrderFlow.Api.http`](src/OrderFlow.Api/OrderFlow.Api.http) and send requests.
Delete `src/OrderFlow.Api/orderflow.db` to reset the database.

## What's here
| Path | What |
|------|------|
| [`tickets/`](tickets/README.md) | **Start here.** Sprint board with 19 tickets: bugs, stories, incident, spike, code review |
| [`src/`](src/) | The API: Clean Architecture across 4 projects (Api / Application / Domain / Infrastructure) |
| [`tests/`](tests/) | xUnit tests (EF Core InMemory) |
| [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md) | Layers, request lifecycle, DI lifetimes |
| [`docs/API_ROUTES.md`](docs/API_ROUTES.md) | Every endpoint + status code cheat sheet |
| [`docs/GLOSSARY.md`](docs/GLOSSARY.md) | ~70 terms you'll hear in week one |
| [`docs/FIRST_WEEK.md`](docs/FIRST_WEEK.md) | Day-one expectations, questions to ask, standup template |
| [`docs/GIT_WORKFLOW.md`](docs/GIT_WORKFLOW.md) | Branch → commit → PR conventions |
| [`docs/code-review/`](docs/code-review/) | A teammate's PR to review (with answer key) |
| `.github/` | CI pipeline + PR template |

## Stack
.NET 10 · ASP.NET Core (controllers + a minimal API) · EF Core + SQLite · xUnit · ProblemDetails · OpenAPI

> At work you'll most likely meet **SQL Server** instead of SQLite. EF Core makes that a one-line swap (`UseSqlServer`).

## How to practise
1. Treat it like a real job: one branch per ticket, small commits, a PR per ticket (even to yourself).
2. For bugs: **reproduce → failing test → fix → test passes.**
3. Write PR descriptions that explain *why*, not just *what*.

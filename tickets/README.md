# 📋 Sprint 42 Board: OrderFlow API

> Work tickets roughly in this order. Each one targets a skill you'll use in week one at a .NET shop.
> Move a ticket by editing its row's Status column. Commit like you would on a real team.

| Ticket | Title | Type | Pri | Pts | Skill | Status |
|--------|-------|------|-----|-----|-------|--------|
| [OF-101](OF-101.md) | Get OrderFlow running locally | Task | High | 1 | Tooling, solution layout | To Do |
| [OF-102](OF-102.md) | Order total ignores quantity | 🐞 Bug | **Crit** | 1 | Domain logic, LINQ, xUnit | To Do |
| [OF-103](OF-103.md) | GET order returns no items | 🐞 Bug | High | 1 | EF Core `Include` | To Do |
| [OF-104](OF-104.md) | Product list skips first page | 🐞 Bug | Med | 1 | Pagination, `Skip/Take` | To Do |
| [OF-105](OF-105.md) | Enforce order status transitions | Story | High | 3 | Domain rules, `[Theory]` | To Do |
| [OF-106](OF-106.md) | Unknown customer → 500 not 404 | 🐞 Bug | Med | 1 | Nullability, error handling | To Do |
| [OF-107](OF-107.md) | Duplicate email → 500 not 409 | 🐞 Bug | Med | 2 | Exceptions, race conditions | To Do |
| [OF-108](OF-108.md) | Validate create-customer requests | Story | Med | 2 | Model validation | To Do |
| [OF-109](OF-109.md) | Check & reserve stock on order | Story | High | 5 | Transactions, concurrency | To Do |
| [OF-110](OF-110.md) | Search / filter / sort products | Story | Med | 3 | `IQueryable`, query params | To Do |
| [OF-111](OF-111.md) | Health check endpoints | Task | Med | 1 | Ops, K8s probes | To Do |
| [OF-112](OF-112.md) | Correlation IDs | Task | Med | 2 | Middleware, logging scopes | To Do |
| [OF-113](OF-113.md) | Raise OrderService test coverage | Tech Debt | Low | 3 | Testing strategy | To Do |
| [OF-114](OF-114.md) | 🔥 /api/customers slow under load | Incident | High | 2 | async/await, thread pool | To Do |
| [OF-115](OF-115.md) | Add SKU + EF migrations | Story | Med | 3 | Migrations, schema changes | To Do |
| [OF-116](OF-116.md) | JWT auth on write endpoints | Story | High | 5 | AuthN / AuthZ | To Do |
| [OF-117](OF-117.md) | SPIKE: cache product catalog | Spike | Low | 2 | Caching, trade-off writing | To Do |
| [OF-118](OF-118.md) | Review a teammate's PR | Task | Med | 1 | Code review | To Do |
| [OF-119](OF-119.md) | Timestamps lose UTC "Z" | 🐞 Bug | Low | 2 | Value converters, dates | To Do |

**Suggested path:** 101 → 102 → 103 → 104 → 106 → 105 → 114 → 118 → then whatever looks fun.

## Ticket types you'll meet
- **Bug**: something is broken. Reproduce first, write a failing test, then fix.
- **Story**: new behaviour for a user. "As a… I want… so that…" plus acceptance criteria (AC).
- **Task**: work with no direct user-facing change (setup, ops).
- **Tech Debt**: improving code that works but is costly to change.
- **Spike**: time-boxed research. The output is a *decision/document*, not production code.
- **Incident**: production problem. Fix, then write a blameless **postmortem**.

## Story points
Relative effort/uncertainty, usually Fibonacci (1, 2, 3, 5, 8, 13). **Not hours.** An 8+ usually gets split.

# OrderFlow API Reference

Base URL (local): `http://localhost:5287` · OpenAPI spec: `GET /openapi/v1.json` · Try it: `src/OrderFlow.Api/OrderFlow.Api.http`

Errors use [RFC 7807 ProblemDetails](https://www.rfc-editor.org/rfc/rfc7807):
```json
{ "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5", "title": "Resource not found", "status": 404, "detail": "Order with id '99' was not found.", "traceId": "00-..." }
```

## Customers
| Method | Route | Body | Success | Errors | Known issues |
|--------|-------|------|---------|--------|--------------|
| GET | `/api/customers` | – | 200 `CustomerDto[]` | | OF-114 |
| GET | `/api/customers/{id}` | – | 200 `CustomerDto` | 404 | OF-106 (returns 500) |
| POST | `/api/customers` | `{ name, email }` | 201 + `Location` header | 400, 409 | OF-107, OF-108 |
| GET | `/api/customers/{id}/orders` | – | 200 `OrderDto[]` | | |

## Products
| Method | Route | Body | Success | Errors | Known issues |
|--------|-------|------|---------|--------|--------------|
| GET | `/api/products?page=1&pageSize=10` | – | 200 `PagedResult<ProductDto>` | | OF-104, OF-110 |
| GET | `/api/products/{id}` | – | 200 `ProductDto` | 404 | |
| POST | `/api/products` | `{ name, price, stockQuantity }` | 201 | 400 | OF-108, OF-116 |
| PUT | `/api/products/{id}` | `{ name, price, stockQuantity, isActive }` | 200 | 400, 404 | OF-116 |

## Orders
| Method | Route | Body | Success | Errors | Known issues |
|--------|-------|------|---------|--------|--------------|
| GET | `/api/orders/{id}` | – | 200 `OrderDto` | 404 | OF-102, OF-103 |
| POST | `/api/orders` | `{ customerId, items: [{ productId, quantity }] }` | 201 | 404, 422 | OF-102, OF-109 |
| PATCH | `/api/orders/{id}/status` | `{ status: "Paid" }` | 200 | 404, 422 | OF-105 |

Order status values: `Pending`, `Paid`, `Shipped`, `Delivered`, `Cancelled`

## Misc
| Method | Route | Notes |
|--------|-------|-------|
| GET | `/api/version` | Minimal API endpoint (not a controller) |
| GET | `/openapi/v1.json` | Dev only |
| GET | `/health/live`, `/health/ready` | Planned in OF-111 |

## HTTP verbs & status codes cheat sheet
| Verb | Meaning | Idempotent? | Typical success |
|------|---------|-------------|-----------------|
| GET | read | ✅ | 200 |
| POST | create / action | ❌ | 201 Created (+ Location) or 200/202 |
| PUT | full replace | ✅ | 200 or 204 |
| PATCH | partial update | usually ❌ | 200 or 204 |
| DELETE | remove | ✅ | 204 |

**4xx = the client's fault, 5xx = our fault.** 400 bad input · 401 not logged in · 403 logged in but not allowed ·
404 not found · 409 conflict (duplicate / concurrency) · 422 valid shape but breaks a business rule · 429 rate-limited ·
500 bug · 502/503/504 an upstream service or dependency failed

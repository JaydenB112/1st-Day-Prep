# PR #231 · Add delete customer endpoint

**Author:** Jordan · **Branch:** `feature/OF-130-delete-customer` → `main` · **Reviewers:** _you_

> **Description:** Support needs a way to delete customers who request account removal. Added an endpoint + service method. Tested locally, works. 👍
>
> **Testing:** Manually hit the endpoint in Postman.

---

### `src/OrderFlow.Api/Controllers/CustomersController.cs`
```diff
+    [HttpGet("delete/{id}")]
+    public IActionResult Delete(int id)
+    {
+        try
+        {
+            customerService.DeleteAsync(id).Wait();
+            return Ok("deleted");
+        }
+        catch (Exception)
+        {
+            return Ok("deleted");
+        }
+    }
```

### `src/OrderFlow.Application/Services/CustomerService.cs`
```diff
+    public async Task DeleteAsync(int id, CancellationToken ct = default)
+    {
+        var customer = await customers.GetByEmailOrIdAsync(id.ToString(), ct);
+        logger.LogInformation($"Deleting customer {customer.Name} ({customer.Email})");
+
+        // remove their orders too so the FK doesn't complain
+        foreach (var order in customer.Orders)
+            orderRepository.Remove(order);
+
+        customers.Remove(customer);
+        await unitOfWork.SaveChangesAsync(ct);
+    }
```

### `src/OrderFlow.Infrastructure/Repositories/CustomerRepository.cs`
```diff
+    public Task<Customer?> GetByEmailOrIdAsync(string value, CancellationToken ct = default) =>
+        db.Customers
+            .FromSqlRaw($"SELECT * FROM Customers WHERE Email = '{value}' OR Id = {value}")
+            .FirstOrDefaultAsync(ct);
```

---

**Your task (OF-118):** write your review comments below before scrolling to the answer key.

```
1.
2.
3.
4.
5.
Verdict:
```

<br><br><br><br><br><br><br><br><br><br>

<details><summary>🔑 Answer key (try first!)</summary>

| # | Severity | Issue |
|---|----------|-------|
| 1 | 🔴 **blocking** | **SQL injection.** `FromSqlRaw` with string interpolation. Use `FromSql($"...")` / `FromSqlInterpolated` (parameterized), or better, plain LINQ: `db.Customers.FirstOrDefaultAsync(c => c.Id == id)`. Also, why search by email when given an int id? |
| 2 | 🔴 **blocking** | **Wrong HTTP verb.** A `GET` that deletes data can be triggered by crawlers, link previews, browser prefetch and caches. Use `[HttpDelete("{id:int}")]` → `DELETE /api/customers/{id}`. |
| 3 | 🔴 **blocking** | **Swallowed exceptions.** `catch (Exception) { return Ok("deleted"); }` lies to the caller and hides failures. Let the global handler deal with it. |
| 4 | 🔴 **blocking** | **Deletes order history.** Financial and order records usually must be retained (audit, tax, Finance's reports). Prefer **soft delete** (`IsDeleted` + global query filter) or anonymize the PII. Product/legal decision, so loop in Sam. |
| 5 | 🔴 **blocking** | **Sync over async.** `.Wait()` blocks a thread (see OF-114). Make the action `async Task<IActionResult>`. |
| 6 | 🟠 **blocking** | **No null check.** Unknown id → `NullReferenceException` → 500 (exactly OF-106). Throw `NotFoundException`. |
| 7 | 🟠 should-fix | **PII in logs + string interpolation.** Logging name/email may breach GDPR policy. Interpolation defeats structured logging. Use `LogInformation("Deleting customer {CustomerId}", id)`. |
| 8 | 🟠 should-fix | **No authorization.** Anyone can delete anyone. Needs `[Authorize(Roles = "admin")]` or similar (OF-116). |
| 9 | 🟡 suggestion | **Response:** `204 No Content` is the conventional DELETE success. `Ok("deleted")` is a string, not JSON. |
| 10 | 🟡 suggestion | **`customer.Orders` isn't loaded** (no `Include`), so the loop does nothing and the FK error comes back. Same root cause as OF-103. |
| 11 | 🟡 suggestion | **No tests**, and "tested in Postman" isn't repeatable. Ask for unit tests (not found, success) and ideally an integration test. |
| 12 | 🔵 nit | `{id}` → `{id:int}` route constraint, consistent with other routes. `CancellationToken` not passed from controller. |

**Verdict: Request Changes.** Blocking security (SQLi), data-loss and correctness issues.

**Tone matters:** "This interpolated `FromSqlRaw` is vulnerable to SQL injection. Could we use LINQ here instead? Happy to pair on it!"
lands much better than "this is wrong". Ask questions, explain the *why*, and point out what's good too.
</details>

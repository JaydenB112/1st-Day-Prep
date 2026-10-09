using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class CustomerRepository(AppDbContext db) : ICustomerRepository
{
    public async Task<List<Customer>> GetAllAsync(CancellationToken ct = default)
    {
        // Simulates a slow downstream call so the sync-over-async issue in OF-114 is noticeable under load.
        await Task.Delay(50, ct);
        return await db.Customers.AsNoTracking().OrderBy(c => c.Id).ToListAsync(ct);
    }

    public Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddAsync(Customer customer, CancellationToken ct = default) =>
        await db.Customers.AddAsync(customer, ct);
}

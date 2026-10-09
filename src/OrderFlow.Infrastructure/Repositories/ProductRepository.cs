using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Common;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = db.Products.AsNoTracking().OrderBy(p => p.Id);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Product>(items, page, pageSize, total);
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default) =>
        db.Products.Where(p => ids.Contains(p.Id)).ToListAsync(ct);

    public async Task AddAsync(Product product, CancellationToken ct = default) =>
        await db.Products.AddAsync(product, ct);
}

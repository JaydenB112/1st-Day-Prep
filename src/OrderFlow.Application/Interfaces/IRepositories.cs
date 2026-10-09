using OrderFlow.Application.Common;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Interfaces;

// Application defines WHAT it needs; Infrastructure decides HOW (EF Core, Dapper, an HTTP API...).

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CancellationToken ct = default);
    Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
}

public interface IProductRepository
{
    Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
}

public interface IOrderRepository
{
    Task<List<Order>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

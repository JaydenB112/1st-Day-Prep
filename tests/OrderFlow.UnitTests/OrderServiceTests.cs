using Microsoft.Extensions.Logging.Abstractions;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Services;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Infrastructure.Persistence;
using OrderFlow.Infrastructure.Repositories;

namespace OrderFlow.UnitTests;

public class OrderServiceTests
{
    private static OrderService CreateSut(AppDbContext db) => new(
        new OrderRepository(db),
        new CustomerRepository(db),
        new ProductRepository(db),
        db,
        NullLogger<OrderService>.Instance);

    [Fact]
    public async Task CreateAsync_NoItems_ThrowsDomainException()
    {
        using var db = TestDb.Create();
        var sut = CreateSut(db);

        await Assert.ThrowsAsync<DomainException>(() =>
            sut.CreateAsync(new CreateOrderRequest(CustomerId: 1, Items: [])));
    }

    [Fact]
    public async Task CreateAsync_UnknownCustomer_ThrowsNotFound()
    {
        using var db = TestDb.Create();
        var sut = CreateSut(db);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            sut.CreateAsync(new CreateOrderRequest(999, [new CreateOrderItemRequest(1, 1)])));
    }

    [Fact(Skip = "OF-102: remove Skip once the total accounts for quantity")]
    public async Task CreateAsync_TotalIsSumOfPriceTimesQuantity()
    {
        using var db = TestDb.Create();
        db.Customers.Add(new Customer { Id = 1, Name = "Test", Email = "t@example.com" });
        db.Products.Add(new Product { Id = 1, Name = "Widget", Price = 10m, StockQuantity = 100 });
        await db.SaveChangesAsync();
        var sut = CreateSut(db);

        var order = await sut.CreateAsync(new CreateOrderRequest(1, [new CreateOrderItemRequest(1, 3)]));

        Assert.Equal(30m, order.Total);
    }

    // TODO (OF-113): add tests for status transitions, stock checks, inactive products...
}

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

    [Fact]
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


    [Fact]
    public async Task CreateAsync_MultipleItems_TotalIsCorrect() 
    {
        // Create a fresh in-memory database so this test is isolated from other tests.
        using var db = TestDb.Create();

        // Add the customer who will place the order.
        db.Customers.Add(new Customer { Id= 1 , Name = "Test", Email = "t@example.com"});

        // Add two products with known prices and enough stock for the requested quantities.
        db.Products.AddRange(
            new Product { Id = 1, Name = "Widget", Price = 10m, StockQuantity = 100 },
            new Product { Id = 2, Name = "Gadget", Price = 5m, StockQuantity = 50 }
        );

        // Persist the customer and products so the order service can retrieve them.
        await db.SaveChangesAsync();

        // Build the service with repositories backed by this test database.
        var sut = CreateSut(db);

        // Create an order with 2 Widgets ($20) and 4 Gadgets ($20).
        var order = await sut.CreateAsync(new CreateOrderRequest(1, [
            new CreateOrderItemRequest(1, 2), // 2 * 10 = 20
            new CreateOrderItemRequest(2, 4)  // 4 * 5 = 20
        ]));

        // Verify the order total is the sum of both line totals: $20 + $20 = $40.
        Assert.Equal(40m, order.Total); // Total should be 20 + 20 = 40
    }

    // TODO (OF-113): add tests for status transitions, stock checks, inactive products...
}

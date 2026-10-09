using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Customers.Any()) return;

        var customers = new List<Customer>
        {
            new() { Name = "Ada Lovelace", Email = "ada@example.com" },
            new() { Name = "Grace Hopper", Email = "grace@example.com" },
            new() { Name = "Linus Torvalds", Email = "linus@example.com" },
        };

        // 25 products so pagination has more than one page at the default size of 10.
        var products = Enumerable.Range(1, 25)
            .Select(i => new Product
            {
                Name = $"Widget {i:D2}",
                Price = 5m + i * 1.25m,
                StockQuantity = i % 7 == 0 ? 0 : 50,
                IsActive = i != 13
            })
            .ToList();

        db.Customers.AddRange(customers);
        db.Products.AddRange(products);
        db.SaveChanges();

        var order = new Order
        {
            CustomerId = customers[0].Id,
            Items =
            [
                new OrderItem { ProductId = products[0].Id, Quantity = 3, UnitPrice = products[0].Price },
                new OrderItem { ProductId = products[1].Id, Quantity = 1, UnitPrice = products[1].Price },
            ]
        };
        var shipped = new Order
        {
            CustomerId = customers[1].Id,
            Items = [new OrderItem { ProductId = products[2].Id, Quantity = 2, UnitPrice = products[2].Price }]
        };
        shipped.ChangeStatus(OrderStatus.Paid);
        shipped.ChangeStatus(OrderStatus.Shipped);

        db.Orders.AddRange(order, shipped);
        db.SaveChanges();
    }
}

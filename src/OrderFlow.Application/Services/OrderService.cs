using OrderFlow.Application.Dtos;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Application.Services;

public interface IOrderService
{
    Task<OrderDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<OrderDto>> GetForCustomerAsync(int customerId, CancellationToken ct = default);
    Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken ct = default);
    Task<OrderDto> UpdateStatusAsync(int id, UpdateOrderStatusRequest request, CancellationToken ct = default);
}

public class OrderService(
    IOrderRepository orders,
    ICustomerRepository customers,
    IProductRepository products,
    IUnitOfWork unitOfWork,
    ILogger<OrderService> logger) : IOrderService
{
    public async Task<OrderDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var order = await orders.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Order), id);
        return ToDto(order);
    }

    public async Task<List<OrderDto>> GetForCustomerAsync(int customerId, CancellationToken ct = default)
    {
        var list = await orders.GetByCustomerIdAsync(customerId, ct);
        return list.Select(ToDto).ToList();
    }

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        if (request.Items.Count == 0)
            throw new DomainException("An order must contain at least one item.");

        _ = await customers.GetByIdAsync(request.CustomerId, ct)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var productLookup = (await products.GetByIdsAsync(productIds, ct)).ToDictionary(p => p.Id);

        var order = new Order { CustomerId = request.CustomerId };
        foreach (var item in request.Items)
        {
            if (!productLookup.TryGetValue(item.ProductId, out var product))
                throw new NotFoundException(nameof(Product), item.ProductId);

            // TODO (OF-109): check product.IsActive and StockQuantity, and decrement stock.
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        await orders.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Created order {OrderId} for customer {CustomerId} with {ItemCount} items",
            order.Id, order.CustomerId, order.Items.Count);

        return ToDto(order);
    }

    public async Task<OrderDto> UpdateStatusAsync(int id, UpdateOrderStatusRequest request, CancellationToken ct = default)
    {
        var order = await orders.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Order), id);

        var previous = order.Status;
        order.ChangeStatus(request.Status);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Order {OrderId} status changed {From} -> {To}", id, previous, request.Status);
        return ToDto(order);
    }

    private static OrderDto ToDto(Order o) => new(
        o.Id,
        o.CustomerId,
        o.Status,
        o.CreatedAtUtc,
        o.CalculateTotal(),
        o.Items.Select(i => new OrderItemDto(i.ProductId, i.Product?.Name, i.Quantity, i.UnitPrice)).ToList());
}

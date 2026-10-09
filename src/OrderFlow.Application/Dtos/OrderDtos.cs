using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.Dtos;

public record OrderItemDto(int ProductId, string? ProductName, int Quantity, decimal UnitPrice);

public record OrderDto(
    int Id,
    int CustomerId,
    OrderStatus Status,
    DateTime CreatedAtUtc,
    decimal Total,
    IReadOnlyList<OrderItemDto> Items);

public record CreateOrderItemRequest(int ProductId, int Quantity);

public record CreateOrderRequest(int CustomerId, List<CreateOrderItemRequest> Items);

public record UpdateOrderStatusRequest(OrderStatus Status);

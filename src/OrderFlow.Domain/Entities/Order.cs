using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; private set; }

    public List<OrderItem> Items { get; set; } = [];

    public decimal CalculateTotal()
    {
        return Items.Sum(i => i.UnitPrice);
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        // TODO (OF-105): enforce valid status transitions.
        // Right now ANY transition is allowed, e.g. Delivered -> Pending, or Cancelled -> Shipped.
        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}

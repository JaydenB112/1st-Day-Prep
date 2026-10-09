namespace OrderFlow.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }

    // Price is copied from the product at time of purchase, so later price changes don't rewrite history.
    public decimal UnitPrice { get; set; }
}

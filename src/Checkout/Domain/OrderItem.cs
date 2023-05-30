namespace Checkout.Domain;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public double Total { get; set; }
}
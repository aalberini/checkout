namespace Checkout.Domain;

public class Order : BaseEntity
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public int ZoneId { get; set; }
    public double ShippingPrice { get; set; }
    public double Taxes { get; set; }
    public double Total { get; set; }
    public List<OrderItem> Items { get; set; } = null!;
}
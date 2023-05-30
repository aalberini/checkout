namespace checkout.Domain.Shipping;

public class Zone1 : ShippingDestination
{
    public override void CalcShippingPrice(double orderTotal, ref double shippingPrice)
    {
        shippingPrice = orderTotal * 0.25;
    }
}
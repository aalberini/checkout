namespace checkout.Domain.Shipping;

public class Zone2 : ShippingDestination
{
    public override void CalcShippingPrice(double orderTotal, ref double shippingPrice)
    {
        shippingPrice = orderTotal * 0.12;
    }
}
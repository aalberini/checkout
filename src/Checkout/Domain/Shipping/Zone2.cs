namespace checkout.Domain.Shipping;

public class Zone2 : ShippingDestination
{
    public void CalcShippingPrice(double orderTotal, ref double shippingPrice)
    {
        shippingPrice = orderTotal * 0.12;
    }
    public override CalcShippingPriceDelegate GetCalcShipping()
    {
        return CalcShippingPrice;
    }
}
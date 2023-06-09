namespace checkout.Domain.Shipping;

public class Zone1 : ShippingDestination
{
    public void CalcShippingPrice(double orderTotal, ref double shippingPrice)
    {
        shippingPrice = orderTotal * 0.25;
    }

    public override CalcShippingPriceDelegate GetCalcShipping()
    {
        return CalcShippingPrice;
    }
}
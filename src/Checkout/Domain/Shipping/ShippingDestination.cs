namespace checkout.Domain.Shipping;

public abstract class ShippingDestination
{
    public virtual void CalcShippingPrice(double orderTotal, ref double shippingPrice) { }
    
    public static ShippingDestination? GetDestinationInfo(int destination)
    {
        return destination switch
        {
            1 => new Zone1(),
            2 => new Zone2(),
            _ => null
        };
    }
}
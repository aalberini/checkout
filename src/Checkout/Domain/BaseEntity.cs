namespace Checkout.Domain;
public class BaseEntity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModifiedByAt { get; set; }
    public DateTime? DeleteAt { get; set; }
}

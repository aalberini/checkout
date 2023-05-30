using Checkout.Domain;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastucture.Persistence;
public class CheckoutDbContext : DbContext
{
    public CheckoutDbContext(DbContextOptions<CheckoutDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}

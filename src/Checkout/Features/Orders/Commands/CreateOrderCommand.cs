using Checkout.Domain;
using checkout.Domain.Shipping;
using Checkout.Exceptions;
using Checkout.Infrastructure.Persistence;
using FluentValidation.Results;
using MediatR;

namespace Checkout.Features.Orders.Commands;

public class CreateOrderCommand : IRequest
{
    public int ClientId { get; set; } = default!;
    public int ZoneId { get; set; } = default!;
    public double Taxes { get; set; }
    public List<OrderItem> Items { get; set; } = null!;
}


public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly CheckoutDbContext _context;

    public CreateOrderCommandHandler(CheckoutDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        double orderTotal = 0;
        double shippingPrice = 0;
        foreach (var item in request.Items)
        {
            var product = await _context.Products.FindAsync(item.Product.ProductId);
            if (product is null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new("Error", $"Product with ID: {item.Product.ProductId} not found")
                });
            }

            item.Product = product;
            item.Total = (item.Product.Price * item.Quantity);
            orderTotal += item.Total;
        }
        ShippingDestination? destination = ShippingDestination.GetDestinationInfo(request.ZoneId);
        if (destination is null)
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new("Error", $"Destination Zone with ID: {request.ZoneId} not found")
            });
        }
        destination?.CalcShippingPrice(orderTotal, ref shippingPrice);
        var newOrder = new Order
        {
            ClientId = request.ClientId,
            ZoneId = request.ZoneId,
            ShippingPrice = shippingPrice,
            Taxes = request.Taxes,
            Items = request.Items
        };

        _context.Orders.Add(newOrder);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
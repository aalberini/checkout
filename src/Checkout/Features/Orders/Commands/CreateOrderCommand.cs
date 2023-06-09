using Checkout.Domain;
using checkout.Domain.Shipping;
using Checkout.Exceptions;
using Checkout.Features.Orders.Queries;
using Checkout.Infrastructure.Persistence;
using FluentValidation.Results;
using MediatR;

namespace Checkout.Features.Orders.Commands;

public class CreateOrderCommand : IRequest<GetOrderQueryResponse>
{
    public int ClientId { get; set; } = default!;
    public int ZoneId { get; set; } = default!;
    public double Taxes { get; set; }
    public List<OrderItem> Items { get; set; } = null!;
}


public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, GetOrderQueryResponse>
{
    private readonly CheckoutDbContext _context;

    public CreateOrderCommandHandler(CheckoutDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetOrderQueryResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var destination = ShippingDestination.GetDestinationInfo(request.ZoneId);
        if (destination is null)
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new("Error", $"Destination Zone with ID: {request.ZoneId} not found")
            });
        }

        double productTotal = 0;
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
            productTotal += item.Total;
        }
        destination.CalcShippingPrice(productTotal, ref shippingPrice);
        var newOrder = new Order
        {
            ClientId = request.ClientId,
            ZoneId = request.ZoneId,
            ShippingPrice = shippingPrice,
            Taxes = request.Taxes,
            Total = productTotal + shippingPrice + request.Taxes,
            Items = request.Items
        };

        _context.Orders.Add(newOrder);

        await _context.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(new GetOrderQueryResponse
        (
            newOrder.OrderId,
            newOrder.ClientId,
            newOrder.ZoneId,
            newOrder.ShippingPrice,
            newOrder.Taxes,
            newOrder.Total,
            newOrder.Items
        ));
    }
}
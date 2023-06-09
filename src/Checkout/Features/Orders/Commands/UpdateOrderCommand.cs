using Checkout.Domain;
using checkout.Domain.Shipping;
using Checkout.Exceptions;
using Checkout.Features.Orders.Queries;
using Checkout.Infrastructure.Persistence;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Features.Orders.Commands;

public class UpdateOrderCommand : IRequest<GetOrderQueryResponse>
{
    public int OrderId { get; set; }
    public int ZoneId { get; set; } = default!;
    public double Taxes { get; set; }
    public List<OrderItem> Items { get; set; } = null!;
}


public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, GetOrderQueryResponse>
{
    private readonly CheckoutDbContext _context;

    public UpdateOrderCommandHandler(CheckoutDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetOrderQueryResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders
            .Where(order => order.OrderId == request.OrderId)
            .Where(order => order.DeleteAt == null)
            .Include(order => order.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefault();

        if (order is null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        var currentProductList = request.Items
            .Select(item => item.Product.ProductId);
        order.Items.Where(item => item.DeleteAt is null).ToList().ForEach(item =>
        {
            if (!currentProductList.Contains(item.Product.ProductId))
            {
                item.DeleteAt = DateTime.UtcNow;
            }
        });
        
        double productTotal = 0;
        double shippingPrice = 0;
        foreach (var item in request.Items)
        {
            var product = _context.Products
                .SingleOrDefaultAsync(product => product.ProductId == item.Product.ProductId, cancellationToken: cancellationToken);
            if (product.Result is null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new("Error", $"Product with ID: {item.Product.ProductId} not found")
                });
            }

            var itemInOrder = order.Items
                .Where(item => item.DeleteAt is null)
                .FirstOrDefault(orderItem => orderItem.Product.ProductId == item.Product.ProductId);

            if (itemInOrder is null)
            {
                itemInOrder = new OrderItem
                {
                    Product = product.Result
                };
                order.Items.Add(itemInOrder);
            }

            itemInOrder.Quantity = item.Quantity;
            itemInOrder.Total = (itemInOrder.Product.Price * itemInOrder.Quantity);
            productTotal += itemInOrder.Total;
        }

        ShippingDestination? destination = ShippingDestination.GetDestinationInfo(request.ZoneId);
        if (destination is null)
        {
            throw new ValidationException(new List<ValidationFailure>
            {
                new("Error", $"Destination Zone with ID: {request.ZoneId} not found")
            });
        }

        destination?.CalcShippingPrice(productTotal, ref shippingPrice);
        order.ZoneId = request.ZoneId;
        order.ShippingPrice = shippingPrice;
        order.Taxes = request.Taxes;
        order.Total = productTotal + shippingPrice + request.Taxes;

        _context.Orders.Update(order);

        await _context.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(new GetOrderQueryResponse
        (
            order.OrderId,
            order.ClientId,
            order.ZoneId,
            order.ShippingPrice,
            order.Taxes,
            order.Total,
            order.Items.Where(item => item.DeleteAt is null).ToList()
        ));
    }
}
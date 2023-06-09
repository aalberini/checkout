using Checkout.Domain;
using Checkout.Exceptions;
using Checkout.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Features.Orders.Queries;

public class GetOrderQuery : IRequest<GetOrderQueryResponse>
{
    public int OrderId { get; set; }
}

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, GetOrderQueryResponse>
{
    private readonly CheckoutDbContext _context;

    public GetOrderQueryHandler(CheckoutDbContext context)
    {
        _context = context;
    }
    public Task<GetOrderQueryResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        // With EF
        var order = _context.Orders
            //.Include(order => order.Items)
            //.ThenInclude(item => item.Product)
            .FirstOrDefault(order => order.OrderId == request.OrderId);

        // With LinQ
        /*
        var order = (from o in _context.Set<Order>()
            where o.OrderId == request.OrderId
            select o).FirstOrDefault();
        */

        if (order != null)
        {
            return Task.FromResult(new GetOrderQueryResponse(
                order.OrderId,
                order.ClientId,
                order.ZoneId,
                order.ShippingPrice,
                order.Taxes,
                order.Total,
                order.Items.Where(item => item.DeleteAt is null).ToList()
            ));
        }
        throw new NotFoundException(nameof(Order), request.OrderId);
    }
}

public record GetOrderQueryResponse(
    int OrderId,
    int ClientId,
    int ZoneId,
    double ShippingPrice,
    double Taxes,
    double Total,
    List<OrderItem> Items
);
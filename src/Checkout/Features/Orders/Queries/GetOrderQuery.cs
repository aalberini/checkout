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
        var order = _context.Orders
            .Where(order => order.OrderId == request.OrderId)
            .Include(order => order.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefault();
        
        /*
        var query = from o in _context.Set<Order>()
            join o0 in _context.Set<OrderItem>()
                on o.OrderId equals p.BlogId into grouping
            select new { b, grouping };
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
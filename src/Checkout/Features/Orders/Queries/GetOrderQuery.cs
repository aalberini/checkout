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

        if (order != null)
        {
            return Task.FromResult(new GetOrderQueryResponse
            {
                OrderId = order.OrderId,
                ClientId = order.ClientId,
                ZoneId = order.ZoneId,
                ShippingPrice = order.ShippingPrice,
                Taxes = order.Taxes,
                Items = order.Items
            });
        }
        throw new NotFoundException(nameof(Order), request.OrderId);
    }
}

public class GetOrderQueryResponse
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public int ZoneId { get; set; }
    public double ShippingPrice { get; set; }
    public double Taxes { get; set; }
    public List<OrderItem> Items { get; set; } = null!;
}

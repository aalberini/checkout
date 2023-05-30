using Checkout.Infrastucture.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Features.Products.Queries;

public class GetProductsQuery : IRequest<List<GetProductsQueryResponse>>
{

}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductsQueryResponse>>
{
    private readonly CheckoutDbContext _context;

    public GetProductsQueryHandler(CheckoutDbContext context)
    {
        _context = context;
    }

    public Task<List<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
        _context.Products
            .AsNoTracking()
            .Select(s => new GetProductsQueryResponse
            {
                ProductId = s.ProductId,
                Description = s.Description,
                Price = s.Price
            })
            .ToListAsync(cancellationToken: cancellationToken);
}

public class GetProductsQueryResponse
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}

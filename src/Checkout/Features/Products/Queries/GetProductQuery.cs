using Checkout.Domain;
using Checkout.Exceptions;
using Checkout.Infrastructure.Persistence;
using MediatR;

namespace Checkout.Features.Products.Queries;

public class GetProductQuery : IRequest<GetProductQueryResponse>
{
    public int ProductId { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductQueryResponse>
{
    private readonly CheckoutDbContext _context;

    public GetProductQueryHandler(CheckoutDbContext context)
    {
        _context = context;
    }
    public async Task<GetProductQueryResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        if (product is not null)
        {
            return new GetProductQueryResponse
            (
                product.ProductId,
                product.Description,
                product.Price
            );
        }
        throw new NotFoundException(nameof(Product), request.ProductId);
    }
}

public record GetProductQueryResponse(
    int ProductId,
    string Description,
    double Price
);

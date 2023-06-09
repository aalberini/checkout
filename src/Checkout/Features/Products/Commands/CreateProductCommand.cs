using Checkout.Domain;
using Checkout.Features.Orders.Queries;
using Checkout.Features.Products.Queries;
using Checkout.Infrastructure.Persistence;
using MediatR;

namespace Checkout.Features.Products.Commands;

public class CreateProductCommand : IRequest<GetProductQueryResponse>
{
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}


public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, GetProductQueryResponse>
{
    private readonly CheckoutDbContext _context;

    public CreateProductCommandHandler(CheckoutDbContext context)
    {
        _context = context;
    }


    public async Task<GetProductQueryResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = new Product
        {
            Description = request.Description,
            Price = request.Price
        };

        _context.Products.Add(newProduct);

        await _context.SaveChangesAsync();

        return await Task.FromResult(new GetProductQueryResponse
        (
            newProduct.ProductId,
            newProduct.Description,
            newProduct.Price
        ));
    }
}
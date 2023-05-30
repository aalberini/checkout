using Checkout.Domain;
using Checkout.Infrastructure.Persistence;
using MediatR;

namespace Checkout.Features.Products.Commands;

public class CreateProductCommand : IRequest
{
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}


public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
{
    private readonly CheckoutDbContext _context;

    public CreateProductCommandHandler(CheckoutDbContext context)
    {
        _context = context;
    }


    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = new Product
        {
            Description = request.Description,
            Price = request.Price
        };

        _context.Products.Add(newProduct);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
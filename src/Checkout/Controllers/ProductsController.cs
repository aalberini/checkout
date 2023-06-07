using Checkout.Features.Products.Commands;
using Checkout.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Consulta los productos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<GetProductsQueryResponse>> GetProducts() => _mediator.Send(new GetProductsQuery());

    /// <summary>
    /// Crea un producto nuevo
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var product = await _mediator.Send(command);

        return Ok(product);
    }

    /// <summary>
    /// Consulta un producto por su ID
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("{ProductId}")]
    public async Task<IActionResult> GetProductById([FromRoute] GetProductQuery query)
    {
        var product = await _mediator.Send(query);

        return Ok(product);
    }
}

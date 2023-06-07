using Checkout.Features.Orders.Commands;
using Checkout.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crea una orden nueva
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var order = await _mediator.Send(command);

        return Ok(order);
    }

    /// <summary>
    /// Consulta una orden por su ID
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("{OrderId}")]
    public Task<GetOrderQueryResponse> GetOrderById([FromRoute] GetOrderQuery query) =>
        _mediator.Send(query);

    /// <summary>
    /// Crea una orden nueva
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{OrderId}")]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command, [FromRoute] int OrderId)
    {
        command.OrderId = OrderId;
        var updatedOrder = await _mediator.Send(command);

        return Ok(updatedOrder);
    }
}

// OrderService/Presentation/Controllers/OrdersController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands.PlaceOrder;

namespace OrderService.Presentation.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            await _mediator.Send(command);
            return Accepted(); // No need for specific ID in this scenario
        }

        // TODO: Add endpoints to view order history etc.
    }
}
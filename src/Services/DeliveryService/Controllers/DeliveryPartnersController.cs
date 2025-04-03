// DeliveryService/Controllers/DeliveryPartnersController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Features.CreateDeliveryPartner;
using DeliveryService.Features.GetAvailableDeliveryPartners;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("api/delivery-partners")]
    public class DeliveryPartnersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryPartnersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryPartnerCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAvailable), new { }, id); // No specific get endpoint for single partner
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var partners = await _mediator.Send(new GetAvailableDeliveryPartnersQuery());
            return Ok(partners);
        }

        // TODO: Add endpoints to manage delivery partners
    }
}
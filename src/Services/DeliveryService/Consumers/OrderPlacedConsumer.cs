// DeliveryService/Consumers/OrderPlacedConsumer.cs
using MassTransit;
using OrderService.Application.Commands.PlaceOrder;
using MediatR;
using DeliveryService.Features.AssignDeliveryPartner;

namespace DeliveryService.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IMediator _mediator;

        public OrderPlacedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            await _mediator.Send(new AssignDeliveryPartnerCommand(context.Message.OrderId));
        }
    }
}
// DeliveryService/Features/AssignDeliveryPartner/AssignDeliveryPartner.cs
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Events;
using BuildingBlocks.AsyncMessaging.Events;
using Marten;
using MediatR;
using DeliveryService.Models;
using MassTransit;

namespace DeliveryService.Features.AssignDeliveryPartner
{
    public record AssignDeliveryPartnerCommand(Guid OrderId) : ICommand;

    public class AssignDeliveryPartnerCommandHandler : ICommandHandler<AssignDeliveryPartnerCommand>
    {
        private readonly IDocumentSession _session;
        private readonly IQuerySession _querySession;
        private readonly IPublishEndpoint _publishEndpoint;

        public AssignDeliveryPartnerCommandHandler(IDocumentSession session, IQuerySession querySession, IPublishEndpoint publishEndpoint)
        {
            _session = session;
            _querySession = querySession;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(AssignDeliveryPartnerCommand request, CancellationToken cancellationToken)
        {
            var availablePartners = await _querySession.Query<DeliveryPartner>()
                .Where(p => p.IsAvailable)
                .ToListAsync(cancellationToken);

            if (availablePartners.Any())
            {
                var partner = availablePartners.First();
                // In a real application, you might have more sophisticated assignment logic
                partner.IsAvailable = false;
                _session.Store(partner);
                await _session.SaveChangesAsync(cancellationToken);

                await _publishEndpoint.Publish(new DeliveryPartnerAssignedEvent(request.OrderId, partner.Id, partner.Name), cancellationToken);
            }
            else
            {
                // Handle scenario where no delivery partner is available
                Console.WriteLine($"No available delivery partner for order: {request.OrderId}");
            }
        }
    }

    public record DeliveryPartnerAssignedEvent(Guid OrderId, Guid DeliveryPartnerId, string DeliveryPartnerName) : IIntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
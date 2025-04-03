// DeliveryService/Features/GetAvailableDeliveryPartners/GetAvailableDeliveryPartners.cs
using BuildingBlocks.CQRS.Queries;
using Marten;
using MediatR;
using DeliveryService.Models;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Features.GetAvailableDeliveryPartners
{
    public record GetAvailableDeliveryPartnersQuery() : IQuery<IEnumerable<DeliveryPartner>>;

    public class GetAvailableDeliveryPartnersQueryHandler : IQueryHandler<GetAvailableDeliveryPartnersQuery, IEnumerable<DeliveryPartner>>
    {
        private readonly IQuerySession _querySession;

        public GetAvailableDeliveryPartnersQueryHandler(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public async Task<IEnumerable<DeliveryPartner>> Handle(GetAvailableDeliveryPartnersQuery request, CancellationToken cancellationToken)
        {
            return await _querySession.Query<DeliveryPartner>()
                .Where(p => p.IsAvailable)
                .ToListAsync(cancellationToken);
        }
    }
}
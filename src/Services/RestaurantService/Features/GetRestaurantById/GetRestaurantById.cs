using BuildingBlocks.CQRS.Queries;
using Marten;
using RestaurantService.Models;

namespace RestaurantService.Features.GetRestaurantById
{
    public record GetRestaurantByIdQuery(Guid Id) : IQuery<Restaurant>;
    public class GetRestaurantByIdQueryHandler(IQuerySession querySession) : IQueryHandler<GetRestaurantByIdQuery, Restaurant>
    {
        public async Task<Restaurant> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            return await querySession.LoadAsync<Restaurant>(request.Id, cancellationToken);
        }
    }
}

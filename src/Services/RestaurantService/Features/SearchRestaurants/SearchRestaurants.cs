using BuildingBlocks.CQRS.Queries;
using Marten;
using RestaurantService.Models;

namespace RestaurantService.Features.SearchRestaurants
{
    public record SearchRestaurantsQuery(string? Name) : IQuery<IEnumerable<Restaurant>>;
    public class SearchRestaurantsQueryHandler(IQuerySession querySession) : IQueryHandler<SearchRestaurantsQuery, IEnumerable<Restaurant>>
    {
        public async Task<IEnumerable<Restaurant>> Handle(SearchRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var query = querySession.Query<Restaurant>();
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = (Marten.Linq.IMartenQueryable<Restaurant>)query.Where(r => r.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
            }
            return await query.ToListAsync(cancellationToken);
        }
    }
}

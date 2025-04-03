    using Marten;
    using Marten.Schema;
    using RestaurantService.Models;

    namespace RestaurantService.Data
    {
        public class RestaurantInitialData : IInitialData
        {
            public async Task Populate(IDocumentStore store, CancellationToken cancellation)
            {
                using var session = store.LightweightSession();
                if (await session.Query<Restaurant>().AnyAsync())
                    return;
                // Marten UPSERT will cater for existing records
                session.Store<Restaurant>(GetPreconfiguredRestaurants());
                await session.SaveChangesAsync();
            }
        private static IEnumerable<Restaurant> GetPreconfiguredRestaurants() => new List<Restaurant>
        {
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = "The Gourmet Spot",
                Menu = new List<FoodItem>
                {
                    new FoodItem { Id = Guid.NewGuid(), Name = "Grilled Salmon", Price = 18.99M },
                    new FoodItem { Id = Guid.NewGuid(), Name = "Caesar Salad", Price = 10.99M },
                    new FoodItem { Id = Guid.NewGuid(), Name = "Cheesecake", Price = 7.50M }
                }
            },
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = "Fast Bites",
                Menu = new List<FoodItem>
                {
                    new FoodItem { Id = Guid.NewGuid(), Name = "Burger Deluxe", Price = 8.99M },
                    new FoodItem { Id = Guid.NewGuid(), Name = "Fries", Price = 3.99M },
                    new FoodItem { Id = Guid.NewGuid(), Name = "Milkshake", Price = 5.50M }
                }
            },
            new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = "Vegan Delights",
                Menu = new List<FoodItem>
                {
                    new FoodItem { Id = Guid.NewGuid(), Name = "Vegan Burrito", Price = 9.99M },
                    new FoodItem { Id = Guid.NewGuid(), Name = "Tofu Stir-fry", Price = 12.99M },
                    new FoodItem { Id = Guid.NewGuid(), Name = "Chia Pudding", Price = 6.50M }
                }
            }
        };
    }
    }

// OrderService/Domain/Aggregates/Order.cs
using OrderService.Domain.Entities;

namespace OrderService.Domain.Aggregates
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid RestaurantId { get; private set; }
        public List<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
        public DateTime OrderDate { get; private set; }

        private Order() { } // Required by EF Core

        public Order(Guid restaurantId)
        {
            Id = Guid.NewGuid();
            RestaurantId = restaurantId;
            OrderDate = DateTime.UtcNow;
        }

        public void AddOrderItem(Guid foodItemId, string foodItemName)
        {
            if (OrderItems.Any(item => item.FoodItemId == foodItemId))
            {
                // Handle existing item logic if needed
                return;
            }
            OrderItems.Add(new OrderItem(foodItemId, foodItemName));
        }
    }
}
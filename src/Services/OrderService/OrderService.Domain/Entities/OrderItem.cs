// OrderService/Domain/Entities/OrderItem.cs
namespace OrderService.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid FoodItemId { get; private set; }
        public string FoodItemName { get; private set; }

        private OrderItem() { } // Required by EF Core

        public OrderItem(Guid foodItemId, string foodItemName)
        {
            Id = Guid.NewGuid();
            FoodItemId = foodItemId;
            FoodItemName = foodItemName;
        }
    }
}
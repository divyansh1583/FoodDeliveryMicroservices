namespace RestaurantService.Models
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<FoodItem> Menu { get; set; } = new List<FoodItem>();
    }

    public class FoodItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

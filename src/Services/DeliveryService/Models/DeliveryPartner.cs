// DeliveryService/Models/DeliveryPartner.cs
namespace DeliveryService.Models
{
    public class DeliveryPartner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsAvailable { get; set; }
    }
}
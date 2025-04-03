using Marten.Schema;
using DeliveryService.Models;
using Marten;

namespace DeliveryService.Data
{
    public class DeliveryPartnerInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if (await session.Query<DeliveryPartner>().AnyAsync())
                return;

            session.Store<DeliveryPartner>(GetPreconfiguredDeliveryPartners());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<DeliveryPartner> GetPreconfiguredDeliveryPartners() => new List<DeliveryPartner>
        {
            new DeliveryPartner
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Phone = "123-456-7890",
                IsAvailable = true
            },
            new DeliveryPartner
            {
                Id = Guid.NewGuid(),
                Name = "Jane Smith",
                Phone = "987-654-3210",
                IsAvailable = false
            },
            new DeliveryPartner
            {
                Id = Guid.NewGuid(),
                Name = "Michael Johnson",
                Phone = "555-123-4567",
                IsAvailable = true
            }
        };

    }
}

namespace BuildingBlocks.AsyncMessaging.Events
{
    public interface IIntegrationEvent
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
    }
}
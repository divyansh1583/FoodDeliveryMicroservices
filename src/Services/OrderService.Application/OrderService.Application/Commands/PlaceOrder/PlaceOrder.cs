// OrderService/Application/Commands/PlaceOrder/PlaceOrder.cs
using BuildingBlocks.AsyncMessaging.Events;
using BuildingBlocks.CQRS.Commands;
using FluentValidation;
using MassTransit;
using OrderService.Domain.Aggregates;
using OrderService.Infrastructure.Database;

namespace OrderService.Application.Commands.PlaceOrder
{
    public record PlaceOrderCommand(Guid RestaurantId, Guid FoodItemId, string FoodItemName) : ICommand;

    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand>
    {
        private readonly OrderDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public PlaceOrderCommandHandler(OrderDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.RestaurantId);
            order.AddOrderItem(request.FoodItemId, request.FoodItemName);

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new OrderPlacedEvent(order.Id, order.RestaurantId, order.OrderItems.Select(oi => new OrderItemInfo(oi.FoodItemId, oi.FoodItemName)).ToList()), cancellationToken);
        }
    }

    public record OrderItemInfo(Guid FoodItemId, string FoodItemName);

    public record OrderPlacedEvent(Guid OrderId, Guid RestaurantId, List<OrderItemInfo> OrderItems) : IIntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.RestaurantId).NotEmpty();
            RuleFor(x => x.FoodItemId).NotEmpty();
            RuleFor(x => x.FoodItemName).NotEmpty().MaximumLength(200);
        }
    }
}
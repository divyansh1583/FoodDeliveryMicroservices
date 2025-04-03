using BuildingBlocks.CQRS.Commands;
using FluentValidation;
using Marten;
using RestaurantService.Models;

namespace RestaurantService.Features.CreateRestaurant
{
    public record CreateRestaurantCommand(string Name) : ICommand<Guid>;
    public class CreateRestaurantCommandHandler(IDocumentSession session) : ICommandHandler<CreateRestaurantCommand, Guid>
    {
        public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = new Restaurant { Name = request.Name };
            session.Store(restaurant);
            await session.SaveChangesAsync(cancellationToken);
            return restaurant.Id;
        }
    }
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        public CreateRestaurantCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}

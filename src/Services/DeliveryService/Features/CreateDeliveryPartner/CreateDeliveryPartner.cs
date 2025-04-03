// DeliveryService/Features/CreateDeliveryPartner/CreateDeliveryPartner.cs
using BuildingBlocks.CQRS.Commands;
using FluentValidation;
using Marten;
using MediatR;
using DeliveryService.Models;

namespace DeliveryService.Features.CreateDeliveryPartner
{
    public record CreateDeliveryPartnerCommand(string Name, string Phone, bool IsAvailable) : ICommand<Guid>;

    public class CreateDeliveryPartnerCommandHandler : ICommandHandler<CreateDeliveryPartnerCommand, Guid>
    {
        private readonly IDocumentSession _session;

        public CreateDeliveryPartnerCommandHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Guid> Handle(CreateDeliveryPartnerCommand request, CancellationToken cancellationToken)
        {
            var partner = new DeliveryPartner
            {
                Name = request.Name,
                Phone = request.Phone,
                IsAvailable = request.IsAvailable
            };
            _session.Store(partner);
            await _session.SaveChangesAsync(cancellationToken);
            return partner.Id;
        }
    }

    public class CreateDeliveryPartnerCommandValidator : AbstractValidator<CreateDeliveryPartnerCommand>
    {
        public CreateDeliveryPartnerCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        }
    }
}
using Domain.Entities;
using Domain.Events.ServiceCharges;
using MediatR;
using Application.Interfaces;
namespace Application.Business.ServiceCharges.Commands.CreateServiceCharge
{
    public record CreateServiceChargeCommand : IRequest<string>
    {
        public double Price { get; set; }
        public string? CreatedBy { get; set; }

    }
    public class CreateServiceChargeCommandHandler : IRequestHandler<CreateServiceChargeCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateServiceChargeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateServiceChargeCommand request, CancellationToken cancellationToken)
        {
            var entity = new ServiceCharge
            {
             Value = request.Price,
             Id = Guid.NewGuid().ToString(),
             CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new ServiceChargeCreatedEvent(entity));
            _context.ServiceCharge.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

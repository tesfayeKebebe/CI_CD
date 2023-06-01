using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
namespace Application.Business.ServiceCharges.Commands.UpdateServiceCharge
{
    public class UpdateServiceChargeCommand : IRequest
    {
        public string? Id { get; set; } = null!;
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateServiceChargeCommandHandler : IRequestHandler<UpdateServiceChargeCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateServiceChargeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateServiceChargeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ServiceCharge.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ServiceCharge), request.Id);
            }
            entity.Value = request.Price;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

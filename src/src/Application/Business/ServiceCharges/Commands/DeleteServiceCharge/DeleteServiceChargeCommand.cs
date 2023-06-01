using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
namespace Application.Business.ServiceCharges.Commands.DeleteServiceCharge
{
    public class DeleteServiceChargeCommand : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteServiceChargeCommandHandler : IRequestHandler<DeleteServiceChargeCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteServiceChargeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteServiceChargeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ServiceCharge.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ServiceCharge), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

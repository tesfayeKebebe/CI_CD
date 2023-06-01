using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.UserAssigns.Commands.UpdateUserAssign
{
    public record UpdateUserAssignCommand   : IRequest
    {
        public string UserId { get; set; }= null !;
        public string UserBranchId { get; set; } = null !;
        public required string Id { get; set; }
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateUserAssignCommandHandler : IRequestHandler<UpdateUserAssignCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserAssignCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserAssignCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.UserAssign.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(UserAssign), request.Id);
            }
            entity.UserId = request.UserId;
            entity.UserBranchId = request.UserBranchId;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

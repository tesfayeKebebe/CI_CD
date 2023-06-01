using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Business.UserAssigns.Commands.DeleteUserAssign
{
    public record DeleteUserAssignCommand : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteUserAssignCommandHandler : IRequestHandler<DeleteUserAssignCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserAssignCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserAssignCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.UserAssign.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(UserAssign), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

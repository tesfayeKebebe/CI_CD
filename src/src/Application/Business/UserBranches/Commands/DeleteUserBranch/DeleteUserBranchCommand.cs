using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Business.UserAssigns.Commands.DeleteUserAssign
{
    public record DeleteUserBranchCommand : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteUserBranchCommandHandler : IRequestHandler<DeleteUserBranchCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserBranchCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.UserBranch.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(UserBranch), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

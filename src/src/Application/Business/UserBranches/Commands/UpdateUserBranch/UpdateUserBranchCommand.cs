using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.UserBranches.Commands.UpdateUserBranch
{
    public record UpdateUserBranchCommand : IRequest
    {
        public string Name { get; set; } = null!;
        public string? Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateUserBranchCommandHandler : IRequestHandler<UpdateUserBranchCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserBranchCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.UserBranch.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(UserBranch), request.Id);
            }
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

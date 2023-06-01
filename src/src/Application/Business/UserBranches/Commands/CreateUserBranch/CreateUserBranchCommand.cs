using Domain.Entities;
using Domain.Events.UserAssigns;
using MediatR;
using Application.Interfaces;
using Domain.Events.UserBranches;

namespace Application.Business.UserBranches.Commands.CreateUserBranch
{
    public record CreateUserBranchCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
        public string? CreatedBy { get; set; }

    }
    public class CreateUserBranchCommandHandler : IRequestHandler<CreateUserBranchCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserBranchCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateUserBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = new UserBranch
            {
               Name = request.Name,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new UserBranchCreatedEvent(entity));
            _context.UserBranch.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

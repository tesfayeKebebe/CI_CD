using Domain.Entities;
using Domain.Events.UserAssigns;
using MediatR;
using Application.Interfaces;

namespace Application.Business.UserAssigns.Commands.CreateUserAssign
{
    public record CreateUserAssignCommand : IRequest<string>
    {
        public string UserId { get; set; }= null !;
        public string UserBranchId { get; set; } = null !;
        public string? CreatedBy { get; set; }
    
    }
    public class CreateUserAssignCommandHandler : IRequestHandler<CreateUserAssignCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserAssignCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateUserAssignCommand request, CancellationToken cancellationToken)
        {
            var entity = new UserAssign
            {
                UserBranchId = request.UserBranchId,
                UserId = request.UserId,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new UserAssignCreatedEvent(entity));
            _context.UserAssign.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

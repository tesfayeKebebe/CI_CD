using Application.Interfaces;
using Domain.Entities;
using Domain.Events.Organizations;
using MediatR;

namespace Application.Business.Organizations.Commands.CreateOrganization
{
    public record CreateOrganizationCommand : IRequest<string>
    {
        public string About { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public required string Telephone { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
    }
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrganizationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var entity = new Organization
            {
                Location = request.Location,
                Telephone = request.Telephone,
                About = request.About,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy,
                Email = request.Email,
            };
            entity.AddDomainEvent(new OrganizationCreatedEvent(entity));
            _context.Organization.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

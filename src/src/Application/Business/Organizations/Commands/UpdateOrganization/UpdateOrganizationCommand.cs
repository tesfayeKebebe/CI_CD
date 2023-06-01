using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.Organizations.Commands.UpdateOrganization
{
    public record UpdateOrganizationCommand    : IRequest
    {
        public string About { get; set; } = null!;
        public string? Id { get; set; } = null!;
        public string? LastModifiedBy { get; set; }
        public  string? Telephone { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
    }
    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateOrganizationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.Organization.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Organization), request.Id);
            }
            entity.About = request.About;
            entity.LastModifiedBy = request.LastModifiedBy;
            entity.Location = request.Location;
            entity.Telephone = request.Telephone;
            entity.Email = request.Email;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

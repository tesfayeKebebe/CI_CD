using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
namespace Application.Business.Labs.Commands.UpdateLab
{
    public record UpdateLabCommand    : IRequest
    {
        public string Name { get; set; } = null!;
        public string? Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }
    }
    public class UpdateLabCommandHandler : IRequestHandler<UpdateLabCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateLabCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateLabCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.Lab.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Lab), request.Id);
            }
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
           await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.TubeTypes.Commands.UpdateTube
{
    public record UpdateTubeTypeCommand    : IRequest
    {
        public string Name { get; set; } = null!;
        public string? Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateTubeTypeCommandHandler : IRequestHandler<UpdateTubeTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTubeTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTubeTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.TubeType.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TubeType), request.Id);
            }
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
           await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
namespace Application.Business.TubeTypes.Commands.DeleteTubeType
{
    public record DeleteTubeTypeCommand  : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteTubeTypeCommandHandler : IRequestHandler<DeleteTubeTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTubeTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTubeTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TubeType.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TubeType), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Business.Labs.Commands.DeleteLab
{
    public record DeleteLabCommand  : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteLabCommandHandler : IRequestHandler<DeleteLabCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteLabCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteLabCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Lab.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Lab), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.SampleTypes.Commands.DeleteLab
{
    public record DeleteSampleTypeCommand  : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteSampleTypeCommandHandler : IRequestHandler<DeleteSampleTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSampleTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSampleTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.SampleType.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(SampleType), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

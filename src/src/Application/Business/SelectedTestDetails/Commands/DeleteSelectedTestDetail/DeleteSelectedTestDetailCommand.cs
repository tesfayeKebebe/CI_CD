using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
namespace Application.Business.SelectedTestDetails.Commands.DeleteSelectedTestDetail
{
    public record DeleteSelectedTestDetailCommand  : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteSelectedTestDetailCommandHandler : IRequestHandler<DeleteSelectedTestDetailCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSelectedTestDetailCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSelectedTestDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.SelectedTestDetail.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(SelectedTestDetail), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

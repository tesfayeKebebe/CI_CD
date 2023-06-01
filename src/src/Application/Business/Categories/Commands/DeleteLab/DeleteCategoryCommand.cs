using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Business.Categories.Commands.DeleteLab
{
    public record DeleteCategoryCommand  : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Category.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

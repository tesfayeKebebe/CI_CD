using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
namespace Application.Business.Categories.Commands.UpdateLab
{
    public record UpdateCategoryCommand  : IRequest
    {
        public string Name { get; set; } = null!;
        public string? Id { get; set; } = null!;
        public string LabId { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Category.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            entity.Name = request.Name;
            entity.LabId = request.LabId;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

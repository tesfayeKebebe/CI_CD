using Domain.Entities;
using Domain.Events.Categories;
using MediatR;
using Application.Interfaces;

namespace Application.Business.Categories.Commands.CreateCategory
{
   public record CreateCategoryCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
        public string LabId { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Category
            {
                Name = request.Name,
                LabId = request.LabId,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new CategoryCreatedEvent(entity));
            _context.Category.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

using Application.Interfaces;
using Domain.Entities;
using Domain.Events.TubeTypes;
using MediatR;
namespace Application.Business.TubeTypes.Commands.CreateTubeType
{
    public record CreateTubeTypeCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
    public class CreateTubeTypeCommandHandler : IRequestHandler<CreateTubeTypeCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateTubeTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateTubeTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = new TubeType()
            {
                Name = request.Name,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new TubeTypeCreatedEvent(entity));
            _context.TubeType.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

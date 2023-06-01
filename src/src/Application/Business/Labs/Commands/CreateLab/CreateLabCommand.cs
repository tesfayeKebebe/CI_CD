using Domain.Entities;
using Domain.Events;
using MediatR;
using Application.Interfaces;
namespace Application.Business.Labs.Commands.CreateLab
{
    public record CreateLabCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
    public class CreateLabCommandHandler : IRequestHandler<CreateLabCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateLabCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateLabCommand request, CancellationToken cancellationToken)
        {
            var entity = new Lab
            {
                Name = request.Name,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new LabCreatedEvent(entity));
            _context.Lab.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

using Application.Interfaces;
using Domain.Entities;
using Domain.Events.SampleTypes;
using MediatR;

namespace Application.Business.SampleTypes.Commands.CreateSampleType
{
    public record CreateSampleTypeCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
    public class CreateSampleTypeCommandHandler : IRequestHandler<CreateSampleTypeCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateSampleTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateSampleTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = new SampleType()
            {
                Name = request.Name,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new SampleTypeCreatedEvent(entity));
            _context.SampleType.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

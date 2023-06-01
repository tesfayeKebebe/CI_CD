using Domain.Entities;
using Domain.Events.TestPrices;
using MediatR;
using Application.Interfaces;

namespace Application.Business.TestPrices.Commands.CreateTestPrice
{
    public record CreateTestPriceCommand : IRequest<string>
    {
        public double Price { get; set; } 
        public string LabTestId { get; set; } = null!;
        public string? CreatedBy { get; set; }
    
    }
    public class CreateTestPriceCommandHandler : IRequestHandler<CreateTestPriceCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateTestPriceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateTestPriceCommand request, CancellationToken cancellationToken)
        {
            var olds = _context.TestPrice.Where(x => x.IsActive && x.LabTestId == request.LabTestId).ToList();
            foreach (var old in olds)
            {
                old.IsActive = false;
                old.LastModifiedBy = request.CreatedBy;
                await _context.SaveChangesAsync(cancellationToken);
            }
            var entity = new TestPrice
            {
                Price = request.Price,
                LabTestId = request.LabTestId,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new TestPriceCreatedEvent(entity));
            _context.TestPrice.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

using Application.Interfaces;
using Domain.Entities;
using Domain.Events.SelectedTestDetails;
using MediatR;
namespace Application.Business.SelectedTestDetails.Commands.CreateSelectedTestDetail
{
    public record CreateSelectedTestDetailCommand : IRequest<string>
    {
        public double Price { get; set; }
        public string LabTestId { get; set; } = null!;
        public string TransactionNumber { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
    public class CreateSelectedTestDetailCommandHandler : IRequestHandler<CreateSelectedTestDetailCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateSelectedTestDetailCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateSelectedTestDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = new SelectedTestDetail
            {
                Price = request.Price,
                LabTestId = request.LabTestId,
                Id = Guid.NewGuid().ToString(),
                TransactionNumber = request.TransactionNumber,
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new SelectedTestDetailCreatedEvent(entity));
            _context.SelectedTestDetail.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

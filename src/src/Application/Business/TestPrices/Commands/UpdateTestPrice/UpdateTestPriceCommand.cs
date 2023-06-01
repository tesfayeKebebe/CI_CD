using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Business.TestPrices.Commands.UpdateTestPrice
{
    public record UpdateTestPriceCommand   : IRequest
    {
        public string? Id { get; set; } = null!;
        public double Price { get; set; }
        public string LabTestId { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateTestPriceCommandHandler : IRequestHandler<UpdateTestPriceCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTestPriceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTestPriceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TestPrice.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TestPrice), request.Id);
            }
            entity.Price = request.Price;
            entity.LabTestId = request.LabTestId;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

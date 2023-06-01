using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.SelectedTestDetails.Commands.UpdateSelectedTestDetail
{
    public record UpdateSelectedTestDetailCommand    : IRequest
    {

        public string? Id { get; set; } = null!;
        public double Price { get; set; }
        public string LabTestId { get; set; } = null!;
        public bool IsActive { get; set; }

    }
    public class UpdateSelectedTestDetailCommandHandler : IRequestHandler<UpdateSelectedTestDetailCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSelectedTestDetailCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateSelectedTestDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.SelectedTestDetail.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(SelectedTestDetail), request.Id);
            }
            entity.Price = request.Price;
            entity.LabTestId = request.LabTestId;
            entity.IsActive = request.IsActive;
           await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;

namespace Application.Business.TestPrices.Commands.DeleteTestPrice
{
    public record DeleteTestPriceCommand : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteTestPriceCommandHandler : IRequestHandler<DeleteTestPriceCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTestPriceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTestPriceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.TestPrice.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TestPrice), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

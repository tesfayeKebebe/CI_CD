using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events.SelectedTestDetails;
using Domain.Events.SelectedTestStatuses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.SelectedTestDetails.Commands.DeleteSelectedTestDetail
{
    public record DeleteSelectedTestDetailByTransactionCommand  : IRequest
    {
        public string? TransactionNumber { get; set; } = null!;
    }
    public class DeleteSelectedTestDetailByTransactionCommandHandler : IRequestHandler<DeleteSelectedTestDetailByTransactionCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSelectedTestDetailByTransactionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSelectedTestDetailByTransactionCommand request, CancellationToken cancellationToken)
        {
            var entities = await _context.SelectedTestDetail.Where(x=>x.TransactionNumber==request.TransactionNumber).ToListAsync(cancellationToken);
          var selectedStatus=  await _context.SelectedTestStatus.FirstOrDefaultAsync(x => x.TransactionNumber == request.TransactionNumber, cancellationToken: cancellationToken);
            if (entities == null || selectedStatus==null)
            {
                throw new NotFoundException(nameof(SelectedTestDetail), request.TransactionNumber);
            }

            foreach (var entity in entities)
            {
                _context.SelectedTestDetail.Remove(entity);
                entity.RemoveDomainEvent(new SelectedTestDetailDeletedEvent(entity));
                await _context.SaveChangesAsync(cancellationToken);
            }
            _context.SelectedTestStatus.Remove(selectedStatus);
            selectedStatus.RemoveDomainEvent(new SelectedTestStatusDeletedEvent(selectedStatus));
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

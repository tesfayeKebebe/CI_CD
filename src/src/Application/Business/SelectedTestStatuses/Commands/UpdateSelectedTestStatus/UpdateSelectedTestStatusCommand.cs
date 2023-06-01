using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.SelectedTestStatuses.Commands.UpdateSelectedTestStatus
{
    public record UpdateSelectedTestStatusCommand    : IRequest<string>
    {
        public string TransactionNumber { get; set; } = null!;
        public TestStatus TestStatus { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateSelectedTestStatusCommandHandler : IRequestHandler<UpdateSelectedTestStatusCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSelectedTestStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(UpdateSelectedTestStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.SelectedTestStatus.FirstOrDefaultAsync(x=>x.TransactionNumber== request.TransactionNumber, cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(SelectedTestStatus), request.TransactionNumber);
            }
            entity.TestStatus = request.TestStatus;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}

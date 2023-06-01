using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.SelectedTestStatuses.Commands.UpdateSelectedTestStatus;

public record UpdateSelectedTestSampleCommand: IRequest
{
    public string TransactionNumber { get; set; } = null!;
    public bool IsSampleTaken { get; set; }
}
public class UpdateSelectedTestSampleCommandHandler : IRequestHandler<UpdateSelectedTestSampleCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSelectedTestSampleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSelectedTestSampleCommand request, CancellationToken cancellationToken)
    {
        var entity = await  _context.SelectedTestStatus.FirstOrDefaultAsync(x=>x.TransactionNumber== request.TransactionNumber, cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(SelectedTestStatus), request.TransactionNumber);
        }
        entity.IsSampleTaken = request.IsSampleTaken;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}
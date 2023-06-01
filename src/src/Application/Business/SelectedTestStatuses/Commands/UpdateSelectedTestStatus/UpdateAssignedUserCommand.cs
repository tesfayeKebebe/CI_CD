using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.SelectedTestStatuses.Commands.UpdateSelectedTestStatus;

public record UpdateAssignedUserCommand: IRequest<string>
{
    public string TransactionNumber { get; set; } = null!;
    public   string? AssignedUser { get; set; }
}
public class UpdateAssignedUserCommandCommandHandler : IRequestHandler<UpdateAssignedUserCommand, string>
{
    private readonly IApplicationDbContext _context;

    public UpdateAssignedUserCommandCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(UpdateAssignedUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await  _context.SelectedTestStatus.FirstOrDefaultAsync(x=>x.TransactionNumber== request.TransactionNumber, cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(SelectedTestStatus), request.TransactionNumber);
        }
        entity.AssignedUser = request.AssignedUser;
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;

    }
}
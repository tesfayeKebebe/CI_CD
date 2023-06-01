using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events.PatientFiles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.PatientFiles.Commands.DeleteCreatePatientFile;

public class DeletePatientFileByIdCommand: IRequest
{
    public string? Id { get; set; } = null!;
}
public class DeletePatientFileByIdCommandHandler : IRequestHandler<DeletePatientFileByIdCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePatientFileByIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePatientFileByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PatientFile.FirstOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(PatientFile), entity.Id);
        }
        _context.PatientFile.Remove(entity);
        entity.RemoveDomainEvent(new PatientFileDeletedEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}
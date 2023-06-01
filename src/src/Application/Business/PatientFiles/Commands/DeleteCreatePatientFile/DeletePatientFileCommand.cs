using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events.PatientFiles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.PatientFiles.Commands.DeleteCreatePatientFile
{
    public record DeletePatientFileCommand : IRequest
    {
        public string? PatientId { get; set; } = null!;
    }
    public class DeletePatientFileCommandHandler : IRequestHandler<DeletePatientFileCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePatientFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePatientFileCommand request, CancellationToken cancellationToken)
        {
            var entities = await _context.PatientFile.Where(x=>x.CreatedBy==request.PatientId).ToListAsync(cancellationToken);
            foreach (var entity in entities)
            {
                if (entity == null)
                {
                    throw new NotFoundException(nameof(PatientFile), entity.Id);
                }
                _context.PatientFile.Remove(entity);
                entity.RemoveDomainEvent(new PatientFileDeletedEvent(entity));
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;

        }
    }
}

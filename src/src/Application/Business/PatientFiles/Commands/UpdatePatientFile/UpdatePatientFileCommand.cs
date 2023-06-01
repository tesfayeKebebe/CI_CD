using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.PatientFiles.Commands.UpdateUserBranch
{
    public record UpdatePatientFileCommand : IRequest
    {
        public string? StoredFileName { get; set; }
        public byte[]? Attachments { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? Id { get; set; } = null!;

    }
    public class UpdatePatientFileCommandHandler : IRequestHandler<UpdatePatientFileCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePatientFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePatientFileCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PatientFile.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(PatientFile), request.Id);
            }
            entity.Attachments = request.Attachments;
            entity.FileName = request.FileName;
            entity.StoredFileName = request.StoredFileName;
            entity.ContentType = request.ContentType;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.LabTestResults.Commands.UpdateLabTestResult
{
    public record UpdateLabTestResultCommand    : IRequest
    {
        public string Id { get; set; } = null!; 
        public string Description { get; set; } = null !;
        public string? LastModifiedBy { get; set; }
        public bool IsCompleted { get; set; }
        public byte[]? Attachments { get; set; }
        public string? StoredFileName { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
    public class UpdateLabTestResultCommandHandler : IRequestHandler<UpdateLabTestResultCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateLabTestResultCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateLabTestResultCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.TestResult.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TestResult), request.Id);
            }
            entity.Description = request.Description;
            entity.LastModifiedBy = request.LastModifiedBy;
            entity.IsCompleted = request.IsCompleted;
            entity.StoredFileName = request.StoredFileName;
            entity.FileName = request.FileName;
            entity.Attachments = request.Attachments;
            entity.ContentType = request.ContentType;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

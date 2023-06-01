using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.LabTestResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Business.LabTestResults.Commands.CreateLabTestResult
{
    public record CreateLabTestResultCommand : IRequest<string>
    {
        public string Description { get; set; } = null !;
        public string PatientId { get; set; } = null!;
        public string TransactionNumber { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public byte[]? Attachments { get; set; }
        public string? StoredFileName { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
    public class CreateCreateLabTestResultCommandHandler : IRequestHandler<CreateLabTestResultCommand, string>
    {
        private readonly IApplicationDbContext _context;
        public CreateCreateLabTestResultCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateLabTestResultCommand request, CancellationToken cancellationToken)
        {
            var entity = new TestResult
            {
              Description = request.Description,
              PatientId= request.PatientId,
              TransactionNumber = request.TransactionNumber,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy, 
                StoredFileName = request.StoredFileName,
                ContentType = request.ContentType,
                Attachments = request.Attachments,
                FileName = request.FileName
            };
            entity.AddDomainEvent(new LabTestResultCreatedEvent(entity));
            _context.TestResult.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}

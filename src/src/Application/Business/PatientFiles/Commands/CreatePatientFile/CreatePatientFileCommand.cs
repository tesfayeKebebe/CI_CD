using System.Globalization;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events.PatientFiles;
using Domain.Events.UserBranches;
using MediatR;

namespace Application.Business.PatientFiles.Commands.CreatePatientFile
{
    public record CreatePatientFileCommand : IRequest<string>
    {
        public string? StoredFileName { get; set; }
        public byte[]? Attachments { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? CreatedBy { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

    }
    public class CreatePatientFileCommandHandler : IRequestHandler<CreatePatientFileCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreatePatientFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreatePatientFileCommand request, CancellationToken cancellationToken)
        {
            var entity = new PatientFile()
            {
              Attachments = request.Attachments,
              ContentType = request.ContentType,
              FileName = request.FileName,
              StoredFileName = request.StoredFileName,
              Id = Guid.NewGuid().ToString(),
              CreatedBy = request.CreatedBy,
              Latitude = Convert.ToString(request.Latitude, CultureInfo.InvariantCulture) , 
              Longitude =Convert.ToString(request.Longitude, CultureInfo.InvariantCulture),
            };
            entity.AddDomainEvent(new PatientFileCreatedEvent(entity));
            _context.PatientFile.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}

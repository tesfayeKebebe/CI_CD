using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
namespace Application.Business.LabTests.Commands.UpdateLabTest
{
    public record UpdateLabTestCommand : IRequest
    {
        public string Name { get; set; } = null!;
        public string? Id { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsFastingRequired { get; set; }
        public string? Description { get; set; }
        public string? LastModifiedBy { get; set; }
        public IList<string> TubeTypeId { get; set; } = new List<string>();
        public  IList<string> SampleTypeId { get; set; }=new List<string>();

    }
    public class UpdateLabTestCommandHandler : IRequestHandler<UpdateLabTestCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateLabTestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateLabTestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.LabTest.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(LabTest), request.Id);
            }

            var samples = _context.LabTestSampleTypeDetail.Where(x => x.LabTestId == request.Id).ToList();
            var tubes = _context.LabTestTubeTypeDetail.Where(x => x.LabTestId == request.Id).ToList();
            _context.LabTestSampleTypeDetail.RemoveRange(samples);
            _context.LabTestTubeTypeDetail.RemoveRange(tubes);
            await _context.SaveChangesAsync(cancellationToken);
            foreach (var sample in request.SampleTypeId)
            {
                var samp = new LabTestSampleTypeDetail
                {
                    LabTestId = entity.Id,
                    SampleTypeId = sample,
                    Id = Guid.NewGuid().ToString(),
                    CreatedBy = request.LastModifiedBy
                };
                _context.LabTestSampleTypeDetail.Add(samp);
                await _context.SaveChangesAsync(cancellationToken);
            }
            foreach (var tube in request.TubeTypeId)
            {
                var tub = new LabTestTubeTypeDetail
                {
                    LabTestId = entity.Id,
                    TubeTypeId = tube,
                    Id = Guid.NewGuid().ToString(), 
                    CreatedBy = request.LastModifiedBy
                };
                _context.LabTestTubeTypeDetail.Add(tub);
                await _context.SaveChangesAsync(cancellationToken);
            }
            entity.Name = request.Name;
            entity.CategoryId = request.CategoryId;
            entity.IsActive = request.IsActive;
            entity.Description = request.Description;
            entity.IsFastingRequired = request.IsFastingRequired;
            entity.LastModifiedBy = request.LastModifiedBy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}

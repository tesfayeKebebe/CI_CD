using Application.Business.LabTests.Queries;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Business.SelectedTestDetails.Queries;


    public record GetSelectedDetailByParentId : IRequest<IEnumerable <SelectedLabTestDetailByParentId>>
    {
        public string ParentId { get; set; } = null!;
    }
    public class GetSelectedDetailByParentIdHandler : IRequestHandler<GetSelectedDetailByParentId, IEnumerable<SelectedLabTestDetailByParentId>>
    {
        private readonly IApplicationDbContext _context;
        public GetSelectedDetailByParentIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
     public async  Task<IEnumerable<SelectedLabTestDetailByParentId>> Handle(GetSelectedDetailByParentId request, CancellationToken cancellationToken)
    {
        var details = new List<SelectedLabTestDetailByParentId>();
        var selectedTestDetails = await _context.SelectedTestDetail.Where(x => x.TransactionNumber == request.ParentId).ToListAsync(cancellationToken: cancellationToken);
        foreach (var selectedDetail in selectedTestDetails)
        {
            var labTest = await _context.LabTest.FirstOrDefaultAsync(x => x.Id == selectedDetail.LabTestId, cancellationToken: cancellationToken);
            if (labTest == null)
            {
                continue;
            }

            {
                var samples = await (from sampleD in _context.LabTestSampleTypeDetail.Where(x => x.LabTestId == labTest.Id)
                    join sample in _context.SampleType on sampleD.SampleTypeId equals sample.Id
                    select new SampleType
                    {
                        Name = sample.Name
                    }).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
                var tubes = await (from tubeD in _context.LabTestTubeTypeDetail.Where(x => x.LabTestId ==  labTest.Id)
                    join tube in _context.TubeType on tubeD.TubeTypeId equals tube.Id
                    select new TubeType
                    {
                        Name = tube.Name
                    }).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
                var sampleBuilder = new StringBuilder();
                foreach (var sample in samples)
                {
                    if ((samples[^1]) == sample)
                    {
                        sampleBuilder.Append(sample.Name);
                    }
                    else
                    {
                        sampleBuilder.Append(sample.Name);
                        sampleBuilder.Append(',');
                    }

                }
                var tubeBuilder = new StringBuilder();
                foreach (var tube in tubes)
                {
                    if ((tubes[^1]) == tube)
                    {
                        tubeBuilder.Append(tube.Name);
                    }
                    else
                    {
                        tubeBuilder.Append(tube.Name);
                        tubeBuilder.Append(',');
                    }

                }
                var detail = new SelectedLabTestDetailByParentId
                {
                    TubeType = tubeBuilder.ToString(),
                    SampleType = sampleBuilder.ToString(),
                    Description = labTest.Description,
                    IsFastingRequired = labTest.IsFastingRequired,
                    Name = labTest.Name
                };
                details.Add(detail);
            }
        }
      
        return details;
    }
}
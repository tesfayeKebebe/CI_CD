using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.LabTests.Queries
{
    public record GetLabTestDetailById: IRequest<LabTestDetailById>
    {
        public string LabTestId { get; set; } = null!;
    }
    public class GetLabTestDetailByIdHandler : IRequestHandler<GetLabTestDetailById, LabTestDetailById>
    {
        private readonly IApplicationDbContext _context;
        public GetLabTestDetailByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<LabTestDetailById> Handle(GetLabTestDetailById request, CancellationToken cancellationToken)
        {
            LabTestDetailById detail = new LabTestDetailById();
           var labTest = await _context.LabTest.FirstOrDefaultAsync(x => x.Id==request.LabTestId);
            if(labTest!=null)
            {
                var samples = await (from sampleD in _context.LabTestSampleTypeDetail.Where(x => x.LabTestId == request.LabTestId)
                               join sample in _context.SampleType on sampleD.SampleTypeId equals sample.Id
                               select new SampleType
                               {
                                   Name = sample.Name
                               }).AsNoTracking().ToListAsync();
                var tubes = await (from tubeD in _context.LabTestTubeTypeDetail.Where(x => x.LabTestId == request.LabTestId)
                                     join tube in _context.TubeType on tubeD.TubeTypeId equals tube.Id
                                     select new TubeType
                                     {
                                         Name = tube.Name
                                     }).AsNoTracking().ToListAsync();
                StringBuilder sampleBuilder = new StringBuilder();
                foreach (var sample in samples)
                {
                    if ((samples[samples.Count-1] ) == sample)
                    {
                        sampleBuilder.Append(sample.Name);
                    }
                    else
                    {
                        sampleBuilder.Append(sample.Name);
                        sampleBuilder.Append(',');
                    }

                }
                StringBuilder tubeBuilder = new StringBuilder();
                foreach (var tube in tubes)
                {
                    if (tubes[^1] == tube)
                    {
                        tubeBuilder.Append(tube.Name);
                    }
                    else
                    {
                        tubeBuilder.Append(tube.Name);
                        tubeBuilder.Append(',');
                    }

                }
                detail.TubeType = tubeBuilder.ToString();
                detail.SampleType = sampleBuilder.ToString();
                detail.Description =labTest.Description;
                detail.IsFastingRequired = labTest.IsFastingRequired;
            }
            return detail;

        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
namespace Application.Business.LabTests.Queries
{
    public record GetLabTest : IRequest<IEnumerable<LabTestDetail>>;
    public class GetLabTestQueryHandler : IRequestHandler<GetLabTest, IEnumerable<LabTestDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetLabTestQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<LabTestDetail>> Handle(GetLabTest request, CancellationToken cancellationToken)
        {
            var entities = await (from test in _context.LabTest
                                  join cat in _context.Category on test.CategoryId equals cat.Id
                                  where !test.IsDeleted
                                  select new LabTestDetail
                                  {
                                      Name = test.Name,
                                      Id = test.Id,
                                      Category = cat.Name,
                                      CategoryId = cat.Id,
                                      IsActive = test.IsActive,
                                      Description = test.Description,
                                      SampleTypeId = _context.LabTestSampleTypeDetail.Where(x=>x.LabTestId==test.Id).Select(y=> y.SampleTypeId).ToList(),
                                      TubeTypeId = _context.LabTestTubeTypeDetail.Where(x=>x.LabTestId==test.Id).Select(y=> y.TubeTypeId).ToList() ,
                                      IsFastingRequired = test.IsFastingRequired
                                  }).AsNoTracking().ToListAsync(cancellationToken);
            return entities;
        }
    }
    }

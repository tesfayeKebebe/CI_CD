using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Business.SampleTypes.Queries
{
    public record GetSampleTypes     : IRequest<IEnumerable<SampleTypeDetail>>;
    public class GetSampleTypeQueries : IRequestHandler<GetSampleTypes, IEnumerable<SampleTypeDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetSampleTypeQueries(IApplicationDbContext context)
        {
            _context = context;
        }

    
        public async Task<IEnumerable<SampleTypeDetail>> Handle(GetSampleTypes request, CancellationToken cancellationToken)
        {
            var entities = await _context.SampleType.AsNoTracking().Where(x=>!x.IsDeleted).Select(x => new SampleTypeDetail
            {
                Name = x.Name,
                Id = x.Id,
                IsActive=x.IsActive
            }).ToListAsync(cancellationToken: cancellationToken);
            return entities;
        }
    }
}

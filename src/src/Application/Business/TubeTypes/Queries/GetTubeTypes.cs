using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.TubeTypes.Queries
{
    public record GetTubeTypes     : IRequest<IEnumerable<TubeTypeDetail>>;
    public class GetTubeTypeQueries : IRequestHandler<GetTubeTypes, IEnumerable<TubeTypeDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetTubeTypeQueries(IApplicationDbContext context)
        {
            _context = context;
        }

    
        public async Task<IEnumerable<TubeTypeDetail>> Handle(GetTubeTypes request, CancellationToken cancellationToken)
        {
            var entities = await _context.TubeType.AsNoTracking().Where(x=>!x.IsDeleted).Select(x => new TubeTypeDetail
            {
                Name = x.Name,
                Id = x.Id,
                IsActive=x.IsActive
            }).ToListAsync();
            return entities;
        }
    }
}

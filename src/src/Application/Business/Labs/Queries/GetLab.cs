
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
namespace Application.Business.Labs.Queries
{
    public record GetLab     : IRequest<IEnumerable<LabDetail>>;
    public class GetLabQueries : IRequestHandler<GetLab, IEnumerable<LabDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetLabQueries(IApplicationDbContext context)
        {
            _context = context;
        }

    
        public async Task<IEnumerable<LabDetail>> Handle(GetLab request, CancellationToken cancellationToken)
        {
            var entities = await _context.Lab.AsNoTracking().Where(x=>!x.IsDeleted).Select(x => new LabDetail
            {
                Name = x.Name,
                Id = x.Id,
                IsActive=x.IsActive
            }).ToListAsync();
            return entities;
        }
    }
}

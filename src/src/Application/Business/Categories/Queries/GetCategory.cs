using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Business.Categories.Queries
{
    public record GetCategory : IRequest<IEnumerable<CategoryDetail>>;
    public class GetCategoryQueryHandler : IRequestHandler<GetCategory, IEnumerable<CategoryDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetCategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryDetail>> Handle(GetCategory request, CancellationToken cancellationToken)
        {
            var entities = await (from cat in _context.Category
                                  join lab in _context.Lab on cat.LabId equals lab.Id
                                  where !cat.IsDeleted
                                  select new CategoryDetail
                                  {
                                      Name = cat.Name,
                                      Id = cat.Id,
                                      Lab = lab.Name,
                                      IsActive = cat.IsActive,
                                      LabId=cat.LabId
                                  }).AsNoTracking().ToListAsync(cancellationToken);
            return entities;
        }
    }
}

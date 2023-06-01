using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
namespace Application.Business.TestPrices.Queries
{
    public record GetTestPrice : IRequest<IEnumerable<TestPriceDetail>>;
    public class GetTestPriceQueryHandler : IRequestHandler<GetTestPrice, IEnumerable<TestPriceDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetTestPriceQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<TestPriceDetail>> Handle(GetTestPrice request, CancellationToken cancellationToken)
        {
            var entities = await (from price in _context.TestPrice
                                  join test in _context.LabTest on price.LabTestId equals test.Id
                                  where !test.IsDeleted && !price.IsDeleted
                                  select new TestPriceDetail
                                  {
                                  LabTest = test.Name,
                                  Price = price.Price  ,
                                  Id=price.Id,
                                  LabTestId = price.LabTestId,  
                                  IsActive = price.IsActive
                                  }).AsNoTracking().ToListAsync(cancellationToken);
            return entities;
        }
    }
}

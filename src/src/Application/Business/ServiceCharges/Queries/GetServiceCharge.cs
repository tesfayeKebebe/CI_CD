using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
namespace Application.Business.ServiceCharges.Queries
{
    public record GetServiceCharge : IRequest<IEnumerable<ServiceChargeDetail>>;
    public class GetServiceChargeQueriesHandler : IRequestHandler<GetServiceCharge, IEnumerable<ServiceChargeDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetServiceChargeQueriesHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<ServiceChargeDetail>> Handle(GetServiceCharge request, CancellationToken cancellationToken)
        {
            var entities = await _context.ServiceCharge.AsNoTracking().Where(x => !x.IsDeleted).Select(x => new ServiceChargeDetail
            {
                Id = x.Id,
                 IsActive = x.IsActive,
                  Price = x.Value
            }).ToListAsync();
            return entities;
        }
    }
}

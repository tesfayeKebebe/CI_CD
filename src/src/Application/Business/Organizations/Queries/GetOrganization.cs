
using Application.Business.BankAccounts.Queries;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.Organizations.Queries
{
    public record GetOrganization     : IRequest<OrganizationDetail>;
    public class GetOrganizationQueries : IRequestHandler<GetOrganization,OrganizationDetail>
    {
        private readonly IApplicationDbContext _context;
        public GetOrganizationQueries(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OrganizationDetail> Handle(GetOrganization request, CancellationToken cancellationToken)
        {
            var entity = await _context.Organization.
                AsNoTracking().
                FirstOrDefaultAsync(cancellationToken);
            if (entity!=null)
            {
                return new OrganizationDetail
                {
                    About = entity.About,
                    Id = entity.Id,
                    Location = entity.Location,
                    Telephone = entity.Telephone,
                    Email = entity.Email, 
                    AccountDetails = await _context.BankAccount.Select(x => new BankAccountDetail
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Account = x.Account
                    }).ToListAsync(cancellationToken),
                        
                };
            }
            return new OrganizationDetail();
        }
    }
}

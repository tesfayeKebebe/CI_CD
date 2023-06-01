
using Application.Business.BankAccounts.Queries;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.BankAccounts.Queries
{
    public record GetBankAccount     : IRequest<List<BankAccountDetail>>;
    public class GetBankAccountQueries : IRequestHandler<GetBankAccount,List<BankAccountDetail>>
    {
        private readonly IApplicationDbContext _context;
        public GetBankAccountQueries(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<BankAccountDetail>> Handle(GetBankAccount request, CancellationToken cancellationToken)
        {
            return await _context.BankAccount.Select(x => new BankAccountDetail
            {
                Id = x.Id,
                Name = x.Name,
                Account = x.Account,
            }).ToListAsync(cancellationToken);

        }
    }
}

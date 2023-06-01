using Application.Business.TestPrices.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Business.UserBranches.Queries
{
    public record GetUserBranch : IRequest<IEnumerable<UserBranchDetail>>;
    public class GetUserAssignQueryHandler : IRequestHandler<GetUserBranch, IEnumerable<UserBranchDetail>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserAssignQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IEnumerable<UserBranchDetail>> Handle(GetUserBranch request, CancellationToken cancellationToken)
        {
            var users =  await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);;
            var entities = await  _context.UserBranch.Where(x=>!x.IsDeleted).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            return entities.Select(item => new UserBranchDetail {Id = item.Id, IsActive = item.IsActive, Name = item.Name}).ToList();
        }
    }
}

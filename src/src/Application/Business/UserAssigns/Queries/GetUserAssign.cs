using Application.Business.TestPrices.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Business.UserAssigns.Queries
{
    public record GetUserAssign : IRequest<IEnumerable<UserAssignDetail>>;
    public class GetUserAssignQueryHandler : IRequestHandler<GetUserAssign, IEnumerable<UserAssignDetail>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserAssignQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IEnumerable<UserAssignDetail>> Handle(GetUserAssign request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);
            var entities =  from user in await _context.UserAssign.Where(x => !x.IsDeleted).ToListAsync(cancellationToken: cancellationToken)
                join branch in _context.UserBranch on user.UserBranchId equals branch.Id
                select new UserAssignDetail
                {
                    Id = user.Id,
                    IsActive = user.IsActive,
                    UserId = user.UserId,
                    UserBranchId = user.UserBranchId,
                    Branch= branch.Name,
                    UserName = users.FirstOrDefault(x => x.Id == user.UserId).UserName,
                    FullName = users.FirstOrDefault(x => x.Id == user.UserId).FullName, 
                    PhoneNumber = users.FirstOrDefault(x => x.Id == user.UserId).PhoneNumber
                };
            return entities;
        }
    }
}

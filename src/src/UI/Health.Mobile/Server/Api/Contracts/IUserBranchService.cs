using Health.Mobile.Server.Models.UserBranches;

namespace Health.Mobile.Server.Api.Contracts;

public interface IUserBranchService
{
    Task<string> CreateUserBranch(UserBranch model);
    Task<string> DeleteUserBranch(string id);
    Task<List<UserBranchDetail>?> GetUserBranch();
    Task<string> UpdateUserBranch(UserBranchDetail model);
}
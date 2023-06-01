using RazorShared.Server.Models.UserBranches;

namespace RazorShared.Server.Api.Contracts;

public interface IUserBranchService
{
    Task<string> CreateUserBranch(UserBranch model);
    Task<string> DeleteUserBranch(string id);
    Task<List<UserBranchDetail>?> GetUserBranch();
    Task<string> UpdateUserBranch(UserBranchDetail model);
}
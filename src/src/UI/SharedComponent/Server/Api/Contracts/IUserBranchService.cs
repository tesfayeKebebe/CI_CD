using SharedComponent.Server.Models.UserBranches;

namespace SharedComponent.Server.Api.Contracts;

public interface IUserBranchService
{
    Task<string> CreateUserBranch(UserBranch model);
    Task<string> DeleteUserBranch(string id);
    Task<List<UserBranchDetail>?> GetUserBranch();
    Task<string> UpdateUserBranch(UserBranchDetail model);
}
using SharedComponent.Server.Models.UserAssigns;

namespace SharedComponent.Server.Api.Contracts;

public interface IUserAssignService
{
    Task<string> CreateUserAssign(UserAssign model);
    Task<string> DeleteUserAssign(string id);
    Task<List<UserAssignDetail>?> GetUserAssign();
    Task<string> UpdateUserAssign(UserAssignDto model);
    Task<string> UpdateUserAssignByService(UserAssignByService model);
}
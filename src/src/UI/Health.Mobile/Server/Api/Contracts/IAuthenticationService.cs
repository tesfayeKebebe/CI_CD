using Health.Mobile.Server.Models.Security;

namespace Health.Mobile.Server.Api.Contracts;

public interface IAuthenticationService
{
    Task<AuthenticationViewModel?> Login(LoginModel model);
    Task <string> Register(RegisterUser model);
    Task <string> RegisterByAdmin(RegisterUser model);
    Task<List<ApplicationUser>> GetAllUsers();
    Task<string> UpdateUser(UserDto? model);
    Task<string> ChangePassword(ChangePasswordModel model);
    Task<string> Delete(string id);
    Task<AuthenticationViewModel?> RefreshToken(RefreshTokenQuery query);
    
   void Logout();
}
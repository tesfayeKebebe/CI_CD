using Web.UI.Server.Models.Security;

namespace Web.UI.Server.Api.Contracts;

public interface IAuthenticationService
{
    Task<AuthenticationViewModel?> Login(LoginModel model);
    Task <string> Register(RegisterUser model);
    Task<AuthenticationViewModel?> RefreshToken(RefreshTokenQuery query);
   void Logout();
}
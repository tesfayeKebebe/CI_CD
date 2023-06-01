using System.ComponentModel.DataAnnotations;

namespace RazorShared.Server.Models.Security;

public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

public class AuthenticationViewModel
{
    public string? Username { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ExpiresIn { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? Type { get; set; }
    public  IList<string>? Roles { get; set; }
    public  string? UserId { get; set; }

}
public class RefreshTokenQuery
{
    public string? Username { get; set; }
    public string? RefreshToken { get; set; }
}

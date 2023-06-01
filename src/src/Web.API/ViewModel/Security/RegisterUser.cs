using System.ComponentModel.DataAnnotations;

namespace Web.API.ViewModel.Security;

public class RegisterUser
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    public string  PhoneNumber { get; set; } = null!;
    public string IdNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? RoleName { get; set; }
    public string? RoleDescription { get; set; }
}

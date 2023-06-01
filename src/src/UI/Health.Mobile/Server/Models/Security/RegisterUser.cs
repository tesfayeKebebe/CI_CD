using System.ComponentModel.DataAnnotations;

namespace Health.Mobile.Server.Models.Security;

public class RegisterUser
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string  PhoneNumber { get; set; } = null!;
    public string IdNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public string? RoleName { get; set; }
    public string? RoleDescription { get; set; }
}
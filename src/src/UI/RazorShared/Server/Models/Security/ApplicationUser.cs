
namespace RazorShared.Server.Models.Security;

public class ApplicationUser 
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string  PhoneNumber { get; set; } = null!;
    public string? IdNumber { get; set; } = null;
    public string FullName { get; set; } = null!;
    public required string Id { get; set; }
    public List<string>? Roles { get; set; }
    public bool IsEnabled { get; set; }
}
public class UserDto
{
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Id { get; set; } = null!;
    public string? UserName { get; set; } = null!;
    public bool IsEnabled { get; set; }
    public string? Email { get; set; }

}
namespace RazorShared.Server.Models.Security;

public class ChangePasswordModel
{
    public string? UserName { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
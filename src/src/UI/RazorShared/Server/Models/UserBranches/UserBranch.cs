
namespace RazorShared.Server.Models.UserBranches;
public class UserBranch
{
    public string Name { get; set; } = null!;
    public string? CreatedBy { get; set; }
}
public class UserBranchDetail
{
    public string? Name { get; set; } = null!;
    public string? Id { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? LastModifiedBy { get; set; }
}
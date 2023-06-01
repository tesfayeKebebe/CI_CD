

namespace Health.Mobile.Server.Models.UserAssigns;
public class UserAssign
{
    public string UserId { get; set; }= null !;
    public string UserBranchId { get; set; } = null !;
    public string? CreatedBy { get; set; }

}
public class UserAssignDetail
{
    public string UserId { get; set; }= null !;
    public string UserBranchId { get; set; } = null !;
    public string Branch { get; set; } = null!;
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Id { get; set; } = null!;
    public bool IsActive { get; set; }

}
public class UserAssignDto
{
    public string UserId { get; set; }= null !;
    public string UserBranchId { get; set; } = null !;
    public string? Id { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? LastModifiedBy { get; set; }
}

public class UserAssignByService
{
    public string UserId { get; set; }= null !;
    public double Longitude { get; set; } 
    public double Latitude { get; set; }
    public string? Token { get; set; }
}
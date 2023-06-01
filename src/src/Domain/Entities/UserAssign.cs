using Domain.Common;

namespace Domain.Entities;

public class UserAssign:BaseAuditableEntity
{
    public string UserId { get; set; }= null !;
    public string UserBranchId { get; set; } = null !;
    public UserBranch UserBranch { get; set; }= null !;
    public string Longitude { get; set; } = "0";
    public string Latitude { get; set; } = "0";
}
using Domain.Common;

namespace Domain.Entities;

public class UserBranch: BaseAuditableEntity
{
    public string Name { get; set; } = null!;
}
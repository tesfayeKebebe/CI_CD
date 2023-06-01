using Domain.Common;

namespace Domain.Entities;

public class TubeType: BaseAuditableEntity
{
    public string Name { get; set; } = null!;
}
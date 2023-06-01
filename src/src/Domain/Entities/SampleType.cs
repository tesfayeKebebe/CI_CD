using Domain.Common;

namespace Domain.Entities;

public class SampleType :  BaseAuditableEntity
{
    public string Name { get; set; } = null!;
}
using Domain.Common;

namespace Domain.Entities;

public class LabTestTubeTypeDetail:BaseAuditableEntity
{
    public string? LabTestId { get; set; }
    public string? TubeTypeId { get; set; }
    public TubeType? TubeType { get; set; }
    public LabTest? LabTest { get; set; }
}
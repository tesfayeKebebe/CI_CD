using Domain.Common;

namespace Domain.Entities;

public class LabTestSampleTypeDetail :BaseAuditableEntity
{
    public string? LabTestId { get; set; }
    public string? SampleTypeId { get; set; }
    public SampleType? SampleType { get; set; }
    public LabTest? LabTest { get; set; }
}
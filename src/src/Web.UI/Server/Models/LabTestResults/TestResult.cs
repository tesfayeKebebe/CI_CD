namespace Web.UI.Server.Models.LabTestResults;
public class TestResult
{
    public string? Description { get; set; } = null !;
    public string PatientId { get; set; } = null!;
    public string ParentId { get; set; } = null!;
}
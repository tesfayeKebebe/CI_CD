namespace RazorShared.Server.Models.LabTestResults;
public class TestResult
{
    public string? Description { get; set; } = null !;
    public string PatientId { get; set; } = null!;
    public string TransactionNumber { get; set; } = null!;
    public string? CreatedBy { get; set; }
}
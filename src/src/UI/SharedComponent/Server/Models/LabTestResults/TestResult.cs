using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace SharedComponent.Server.Models.LabTestResults;
public class TestResult
{
    public string? Description { get; set; } = null !;
    public string PatientId { get; set; } = null!;
    public string TransactionNumber { get; set; } = null!;
    public string? CreatedBy { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? StoredFileName { get; set; }
}
using Domain.Common;

namespace Domain.Entities;

public class TestResult: BaseAuditableEntity
{
    public string Description { get; set; } = null !;
    public string PatientId { get; set; } = null!;
    public string TransactionNumber { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public string? ApprovedBy { get; set; }
    public string? Reason { get; set; }
    public string? StoredFileName { get; set; }
    public byte[]? Attachments { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
}
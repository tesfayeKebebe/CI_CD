namespace SharedComponent.Server.Models.LabTestResults
{
    public class TestResultDetail
    {
        public string? Description { get; set; } 
        public string? Id { get; set; } 
        public string? LastModifiedBy { get; set; }
        public bool IsCompleted { get; set; }
        public string? Reason { get; set; }
        public string? StoredFileName { get; set; }
        public string TransactionNumber { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public string PatientId { get; set; }= null!;
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        
        
    }

    public class LabTestResultApproval
    {
        public string? Id { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string? Reason { get; set; } = null !;
        public string? ApprovedBy { get; set; }
    }
}

namespace RazorShared.Server.Models.LabTestResults
{
    public class TestResultDetail
    {
        public string? Description { get; set; } 
        public string? Id { get; set; } 
        public string? LastModifiedBy { get; set; }
        public bool IsCompleted { get; set; }
        public string? Reason { get; set; }
        
    }

    public class LabTestResultApproval
    {
        public string Id { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string Reason { get; set; } = null !;
        public string? ApprovedBy { get; set; }
    }
}

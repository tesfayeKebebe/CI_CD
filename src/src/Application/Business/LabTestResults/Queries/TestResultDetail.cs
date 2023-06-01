namespace Application.Business.LabTestResults.Queries
{
    public class TestResultDetail
    {
        public string Description { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string? Reason { get; set; } 
        public string? StoredFileName { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        
    }
}

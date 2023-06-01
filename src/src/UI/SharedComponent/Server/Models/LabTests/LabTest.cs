namespace SharedComponent.Server.Models.LabTests
{
    public class LabTestDetail
    {
        public string? Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsFastingRequired { get; set; }
        public string? Description { get; set; }
        public IEnumerable<string> TubeTypeId { get; set; } = new List<string>();
        public  IEnumerable<string> SampleTypeId { get; set; }= new List<string>();
        public string? LastModifiedBy { get; set; }
    }
    public class LabTest
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public bool IsFastingRequired { get; set; }
        public string? Description { get; set; }
        public IEnumerable<string>? TubeTypeId { get; set; } 
        public  IEnumerable<string>? SampleTypeId { get; set; }
        public string? CreatedBy { get; set; }
    }

}

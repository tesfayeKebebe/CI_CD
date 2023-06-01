namespace Application.Business.LabTests.Queries
{
    public class LabTestDetail
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsFastingRequired { get; set; }
        public string? Description { get; set; }
        public IList<string> TubeTypeId { get; set; } = new List<string>();
        public  IList<string> SampleTypeId { get; set; }= new List<string>();
    }
}

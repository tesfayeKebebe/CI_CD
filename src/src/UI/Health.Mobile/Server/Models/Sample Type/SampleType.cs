namespace Health.Mobile.Server.Models.Sample_Type
{
    public class SampleType
    {
        public string Name { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
    public class SampleTypeDetail
    {
        public string? Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}

namespace Web.UI.Server.Models.Sample_Type
{
    public class SampleType
    {
        public string Name { get; set; } = null!;
    }
    public class SampleTypeDetail
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}

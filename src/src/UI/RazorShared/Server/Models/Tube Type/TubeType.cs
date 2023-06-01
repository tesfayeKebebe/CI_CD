namespace RazorShared.Server.Models.Tube_Type
{
    public class TubeType
    {
        public string Name { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
    public class TubeTypeDetail
    {
        public string? Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}

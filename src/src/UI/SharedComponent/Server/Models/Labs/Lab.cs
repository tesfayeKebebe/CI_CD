namespace SharedComponent.Server.Models.Labs
{
    public class LabDetail
    {
        public string? Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }
    }

    public class Lab
    {
        public string Name { get; set; } = null!; 
        public string? CreatedBy { get; set; }
    }

}

namespace Web.UI.Server.Models.Tube_Type
{
    public class TubeType
    {
        public string Name { get; set; } = null!;
    }
    public class TubeTypeDetail
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}

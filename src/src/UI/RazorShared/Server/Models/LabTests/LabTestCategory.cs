namespace RazorShared.Server.Models.LabTests
{
    public class LabTestCategory
    {
        public string? Test { get; set; }
        public string Name { get; set; } = null!;
        public string Lab { get; set; } = null!;
        public string Id { get; set; } = null!;
        public double Price { get; set; }
        public ICollection<LabTestPriceDetail> LabTests { get; set; } = new List<LabTestPriceDetail>();
    }


}
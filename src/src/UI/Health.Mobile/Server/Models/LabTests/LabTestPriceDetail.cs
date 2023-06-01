namespace Health.Mobile.Server.Models.LabTests
{
    public class LabTestPriceDetail
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public double Price { get; set; }
        public  bool Selected { get; set; }
    }
}
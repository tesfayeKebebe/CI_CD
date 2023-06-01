namespace SharedComponent.Server.Models.LabTests
{
    public class LabTestCategory
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
        public LabTestPriceDetail LabTest { get; set; } = new LabTestPriceDetail();
    }


}
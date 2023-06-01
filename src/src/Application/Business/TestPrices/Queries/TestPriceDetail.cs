namespace Application.Business.TestPrices.Queries
{
    public class TestPriceDetail
    {
        public double Price { get; set; }
        public string LabTest { get; set; } = null!;
        public string? Id { get; set; }
        public string LabTestId { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}

namespace Application.Business.LabTests.Queries;
public class LabCategoryDetail2
{
    public string Name { get; set; } = null!;
    public string Id { get; set; } = null!;
    public LabTestPriceDetail2 LabTest { get; set; } = new LabTestPriceDetail2();
}
using SharedComponent.Server.Models.LabTests;

namespace SharedComponent.Pages.ViewModel;

public class LabTestCategoryViewModel
{
    public string Name { get; set; } = null!;
    public string Lab { get; set; } = null!;
    public string Id { get; set; } = null!;
    public double Price { get; set; }
    public int Count { get; set; }
    public Dictionary<string,LabTestPriceDetail> LabTests { get; set; } = new Dictionary<string, LabTestPriceDetail>();
}
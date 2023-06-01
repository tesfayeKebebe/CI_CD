namespace SharedComponent.Server.Models.SelectedTestDetails;

public class SelectedTestDetailModel
{
    public double Price { get; set; }
    public string LabTestId { get; set; } = null!;
    public string? CreatedBy { get; set; }
    public string TransactionNumber { get; set; } = null!;
}
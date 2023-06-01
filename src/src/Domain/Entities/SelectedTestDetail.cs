using Domain.Common;

namespace Domain.Entities;

public class SelectedTestDetail : BaseAuditableEntity
{
    public double Price { get; set; }
    public string TransactionNumber { get; set; } = null!;
    public string LabTestId { get; set; } = null!;
    public LabTest LabTest { get; set; } = null!;
    
}
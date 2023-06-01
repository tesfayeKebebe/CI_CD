using Domain.Common;
using Domain.Enums;
namespace Domain.Entities;

public class SelectedTestStatus: BaseAuditableEntity
{
    public string TransactionNumber { get; set; } = null!;
    public double Amount { get; set; }
    public TestStatus TestStatus { get; set; }
    public  string Longitude { get; set; }= "0";
    public  string Latitude { get; set; }= "0";
    public   string? AssignedUser { get; set; }
    public bool IsSampleTaken { get; set; }
    public int SampleNumber { get; set; }
}
namespace SharedComponent.Server.Models.SelectedTestDetails;

public class SelectedTestDetail 
{
    public double Amount { get; set; }
    public string TransactionNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string PatientName { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string? IdNumber { get; set; } = null!;
    public DateTime Date { get; set; }
    public string PatientId { get; set; }= null!;
    public bool IsCompleted { get; set; }
    public string? DoneBy { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string? AssignedUser { get; set; }
    public string? AssignUserId { get; set; }
    public bool IsSampleTaken { get; set; }
    public int SampleNumber { get; set; }

}
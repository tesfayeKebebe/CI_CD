namespace Web.UI.Server.Models.SelectedTestDetails;

public class SelectedTestDetail 
{
    public double Amount { get; set; }
    public string ParentId  { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string PatientName { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string? IdNumber { get; set; } = null!;
    public DateTime Date { get; set; }
    public string PatientId { get; set; }= null!;

}
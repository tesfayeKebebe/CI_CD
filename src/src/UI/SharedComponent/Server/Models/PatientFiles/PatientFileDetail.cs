namespace SharedComponent.Server.Models.PatientFiles;

public class PatientFileDetail
{
    public string? PatientName { get; set; } = null!;
    public string? PatientId { get; set; } = null!;
    public string? Id { get; set; } = null!;
    public string? StoredFileName { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? LastModifiedBy { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string? PhoneNumber { get; set; }
}
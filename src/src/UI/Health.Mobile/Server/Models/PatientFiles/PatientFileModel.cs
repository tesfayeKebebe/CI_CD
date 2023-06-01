namespace Health.Mobile.Server.Models.PatientFiles;

public class PatientFileModel
{
    public string? StoredFileName { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? CreatedBy { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    
}
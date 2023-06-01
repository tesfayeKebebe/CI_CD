using Domain.Common;

namespace Domain.Entities;

public class PatientFile: BaseAuditableEntity
{
    public string? StoredFileName { get; set; }
    public byte[]? Attachments { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public  string Longitude { get; set; }= "0";
    public  string Latitude { get; set; }= "0";
}
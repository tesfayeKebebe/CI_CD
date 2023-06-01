namespace SharedComponent.Server.Models;

public class FileClass
{
    public bool Uploaded { get; set; }
    public string? FileName { get; set; }
    public string? StoredFileName { get; set; }
    public string? ContentType { get; set; }
    public string? Base64data { get; set; }
    public string? CreatedBy { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}


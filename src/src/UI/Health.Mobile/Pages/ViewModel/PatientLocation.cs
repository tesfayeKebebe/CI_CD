namespace Health.Mobile.Pages.ViewModel;

public class PatientLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PatientName { get; set; }
    public string? TransactionNumber { get; set; }
    public bool IsSampleTaken { get; set; }
    public int SampleNumber { get; set; }
}
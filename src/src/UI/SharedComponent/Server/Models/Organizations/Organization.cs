namespace SharedComponent.Server.Models.Organizations;

public class Organization
{
    public string About { get; set; } = null!;
    public string? CreatedBy { get; set; }
    public required string Telephone { get; set; }
    public string? Location { get; set; }
    public string? Email { get; set; }
    public string? BankAccount { get; set; }
}
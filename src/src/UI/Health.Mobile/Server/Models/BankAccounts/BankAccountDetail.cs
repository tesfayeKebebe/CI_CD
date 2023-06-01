namespace Health.Mobile.Server.Models.BankAccounts;

public class BankAccountDetail
{
    public string Name { get; set; } = null!;
    public long Account { get; set; }
    public string? Id { get; set; } = null!;
    public string? LastModifiedBy { get; set; }
}
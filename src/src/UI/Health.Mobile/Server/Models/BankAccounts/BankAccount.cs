namespace Health.Mobile.Server.Models.BankAccounts;

public class BankAccount
{
    public string Name { get; set; } = null!;
    public long Account { get; set; }
    public string? CreatedBy { get; set; }
}
using Health.Mobile.Server.Models.BankAccounts;
using Health.Mobile.Server.Models.Categories;

namespace Health.Mobile.Server.Api.Contracts;

public interface IBankAccountService
{
    Task<List<BankAccountDetail>> GetBankAccounts();
    Task<string> CreateBankAccount(BankAccount model);
    Task<string> UpdateBankAccount(BankAccountDetail model);
}
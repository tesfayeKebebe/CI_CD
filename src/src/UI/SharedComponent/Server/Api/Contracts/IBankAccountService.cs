using SharedComponent.Server.Models.BankAccounts;
using SharedComponent.Server.Models.Categories;

namespace SharedComponent.Server.Api.Contracts;

public interface IBankAccountService
{
    Task<List<BankAccountDetail>> GetBankAccounts();
    Task<string> CreateBankAccount(BankAccount model);
    Task<string> UpdateBankAccount(BankAccountDetail model);
}
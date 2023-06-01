using Application.Business.BankAccounts.Commands.CreateBankAccount;
using FluentValidation;

namespace Application.Business.BankAccounts.Commands.CreateBankAccount
{
    public class CreateBankAccountCommandValidation   : AbstractValidator<CreateBankAccountCommand>
    {
        public CreateBankAccountCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Account).NotEmpty();
        }
    }
}

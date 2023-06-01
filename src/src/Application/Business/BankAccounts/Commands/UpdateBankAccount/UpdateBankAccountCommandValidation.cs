using Application.Business.BankAccounts.Commands.UpdateBankAccount;
using FluentValidation;

namespace Application.Business.BankAccounts.Commands.UpdateBankAccount
{
    public class UpdateBankAccountCommandValidation : AbstractValidator<UpdateBankAccountCommand>
    {
        public UpdateBankAccountCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Account).NotEmpty();     
            RuleFor(x => x.Id).NotEmpty();
            
        }

    }
}

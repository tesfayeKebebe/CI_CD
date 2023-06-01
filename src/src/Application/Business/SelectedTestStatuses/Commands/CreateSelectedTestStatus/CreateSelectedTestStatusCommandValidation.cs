using FluentValidation;
namespace Application.Business.SelectedTestStatuses.Commands.CreateSelectedTestStatus
{
    public class CreateSelectedTestStatusCommandValidation   : AbstractValidator<CreateSelectedTestStatusCommand>
    {
        public CreateSelectedTestStatusCommandValidation()
        {
            RuleFor(x => x.TransactionNumber).NotEmpty();
        }
    }
}

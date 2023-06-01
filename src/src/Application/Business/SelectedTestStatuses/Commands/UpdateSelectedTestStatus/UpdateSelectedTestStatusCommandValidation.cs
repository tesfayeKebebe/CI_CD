using FluentValidation;
namespace Application.Business.SelectedTestStatuses.Commands.UpdateSelectedTestStatus
{
    public class UpdateSelectedTestDetailCommandValidation : AbstractValidator<UpdateSelectedTestStatusCommand>
    {
        public UpdateSelectedTestDetailCommandValidation()
        {
            RuleFor(x => x.TestStatus).NotEmpty();
            RuleFor(x => x.TransactionNumber).NotEmpty();
        }

    }
}

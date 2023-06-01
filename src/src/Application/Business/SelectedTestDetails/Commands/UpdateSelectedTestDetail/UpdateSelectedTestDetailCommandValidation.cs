using FluentValidation;

namespace Application.Business.SelectedTestDetails.Commands.UpdateSelectedTestDetail
{
    public class UpdateSelectedTestDetailCommandValidation : AbstractValidator<UpdateSelectedTestDetailCommand>
    {
        public UpdateSelectedTestDetailCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.LabTestId).NotEmpty();
        }

    }
}

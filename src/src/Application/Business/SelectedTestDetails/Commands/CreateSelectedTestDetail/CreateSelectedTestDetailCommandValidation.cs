using FluentValidation;
namespace Application.Business.SelectedTestDetails.Commands.CreateSelectedTestDetail
{
    public class CreateSelectedTestDetailCommandValidation   : AbstractValidator<CreateSelectedTestDetailCommand>
    {
        public CreateSelectedTestDetailCommandValidation()
        {
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.LabTestId).NotEmpty();
        }
    }
}

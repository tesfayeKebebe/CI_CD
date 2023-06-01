using FluentValidation;

namespace Application.Business.LabTestResults.Commands.UpdateLabTestResult
{
    public class UpdateLabTestResultCommandValidation : AbstractValidator<UpdateLabTestResultCommand>
    {
        public UpdateLabTestResultCommandValidation()
        {
            RuleFor(x => x.Description).NotEmpty();
        }

    }
}

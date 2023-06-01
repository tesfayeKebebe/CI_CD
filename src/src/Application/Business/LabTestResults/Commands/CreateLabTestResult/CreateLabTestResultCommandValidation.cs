using FluentValidation;

namespace Application.Business.LabTestResults.Commands.CreateLabTestResult
{
    public class CreateLabTestResultCommandValidation   : AbstractValidator<CreateLabTestResultCommand>
    {
        public CreateLabTestResultCommandValidation()
        {
            RuleFor(x => x.PatientId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}

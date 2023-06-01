using FluentValidation;

namespace Application.Business.PatientFiles.Commands.CreatePatientFile
{
    public class CreatePatientFileCommandValidation : AbstractValidator<CreatePatientFileCommand>
    {
        public CreatePatientFileCommandValidation()
        {
            RuleFor(x => x.CreatedBy).NotNull();
        }
    }
}

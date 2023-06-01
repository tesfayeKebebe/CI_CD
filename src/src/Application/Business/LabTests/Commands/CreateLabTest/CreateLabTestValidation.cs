using FluentValidation;
namespace Application.Business.LabTests.Commands.CreateLabTest
{
    public class CreateLabTestValidation : AbstractValidator<CreateLabTestCommand>
    {
        public CreateLabTestValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}

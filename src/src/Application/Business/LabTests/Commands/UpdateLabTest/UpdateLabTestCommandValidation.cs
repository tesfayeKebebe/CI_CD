using FluentValidation;
namespace Application.Business.LabTests.Commands.UpdateLabTest
{
    public class UpdateLabTestCommandValidation : AbstractValidator<UpdateLabTestCommand>
    {
        public UpdateLabTestCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}

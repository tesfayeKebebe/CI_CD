using FluentValidation;
namespace Application.Business.TestPrices.Commands.CreateTestPrice
{
    public class CreateTestPriceCommandValidation : AbstractValidator<CreateTestPriceCommand>
    {
        public CreateTestPriceCommandValidation()
        {
            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.LabTestId).NotNull();

        }
    }
}

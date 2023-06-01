using Application.Business.TestPrices.Commands.CreateTestPrice;
using FluentValidation;
namespace Application.Business.TestPrices.Commands.UpdateTestPrice
{
    public class UpdateTestPriceCommandValidation : AbstractValidator<CreateTestPriceCommand>
    {
        public UpdateTestPriceCommandValidation()
        {
            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.LabTestId).NotNull();

        }
    }
}

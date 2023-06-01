using FluentValidation;
namespace Application.Business.ServiceCharges.Commands.CreateServiceCharge
{
    public class CreateServiceChargeValidation  : AbstractValidator<CreateServiceChargeCommand>
    {
        public CreateServiceChargeValidation()
        {
            RuleFor(x => x.Price).NotNull();
        }
    }
}

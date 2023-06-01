using FluentValidation;
namespace Application.Business.ServiceCharges.Commands.UpdateServiceCharge
{
    public class UpdateServiceChargeCommandValidation : AbstractValidator<UpdateServiceChargeCommand>
    {
        public UpdateServiceChargeCommandValidation()
        {
            RuleFor(x => x.Price).NotNull();
        }
    }
}

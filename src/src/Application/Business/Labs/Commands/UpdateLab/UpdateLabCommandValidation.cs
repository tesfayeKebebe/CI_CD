using FluentValidation;
namespace Application.Business.Labs.Commands.UpdateLab
{
    public class UpdateLabCommandValidation : AbstractValidator<UpdateLabCommand>
    {
        public UpdateLabCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }

    }
}

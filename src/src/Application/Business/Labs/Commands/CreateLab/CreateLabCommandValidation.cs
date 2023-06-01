using FluentValidation;
namespace Application.Business.Labs.Commands.CreateLab
{
    public class CreateLabCommandValidation   : AbstractValidator<CreateLabCommand>
    {
        public CreateLabCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}

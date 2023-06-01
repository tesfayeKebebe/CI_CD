using FluentValidation;
namespace Application.Business.TubeTypes.Commands.CreateTubeType
{
    public class CreateTubeTypeCommandValidation   : AbstractValidator<CreateTubeTypeCommand>
    {
        public CreateTubeTypeCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}

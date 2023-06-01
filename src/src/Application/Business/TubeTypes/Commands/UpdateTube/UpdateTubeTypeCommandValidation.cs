using FluentValidation;
namespace Application.Business.TubeTypes.Commands.UpdateTube
{
    public class UpdateTubeTypeCommandValidation : AbstractValidator<UpdateTubeTypeCommand>
    {
        public UpdateTubeTypeCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }

    }
}

using FluentValidation;

namespace Application.Business.SampleTypes.Commands.CreateSampleType
{
    public class CreateSampleTypeCommandValidation   : AbstractValidator<CreateSampleTypeCommand>
    {
        public CreateSampleTypeCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}

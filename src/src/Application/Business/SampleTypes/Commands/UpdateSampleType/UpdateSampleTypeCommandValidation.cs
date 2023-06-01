using Application.Business.SampleTypes.Commands.UpdateLab;
using FluentValidation;

namespace Application.Business.SampleTypes.Commands.UpdateSampleType
{
    public class UpdateSampleTypeCommandValidation : AbstractValidator<UpdateSampleTypeCommand>
    {
        public UpdateSampleTypeCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }

    }
}

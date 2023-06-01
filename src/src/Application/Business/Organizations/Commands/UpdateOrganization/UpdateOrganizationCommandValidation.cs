using FluentValidation;

namespace Application.Business.Organizations.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandValidation : AbstractValidator<UpdateOrganizationCommand>
    {
        public UpdateOrganizationCommandValidation()
        {
            RuleFor(x => x.About).NotEmpty();
        }

    }
}

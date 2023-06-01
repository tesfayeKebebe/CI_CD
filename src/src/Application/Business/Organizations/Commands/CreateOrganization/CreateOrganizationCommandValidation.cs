using FluentValidation;

namespace Application.Business.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationCommandValidation   : AbstractValidator<CreateOrganizationCommand>
    {
        public CreateOrganizationCommandValidation()
        {
            RuleFor(x => x.About).NotEmpty();
        }
    }
}

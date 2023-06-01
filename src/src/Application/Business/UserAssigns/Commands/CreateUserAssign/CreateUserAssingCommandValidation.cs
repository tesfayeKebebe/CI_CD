using Application.Business.UserBranches.Commands.CreateUserBranch;
using FluentValidation;

namespace Application.Business.UserAssigns.Commands.CreateUserAssign
{
    public class CreateUserAssignCommandValidation : AbstractValidator<CreateUserBranchCommand>
    {
        public CreateUserAssignCommandValidation()
        {
            RuleFor(x => x.Name).NotNull();

        }
    }
}

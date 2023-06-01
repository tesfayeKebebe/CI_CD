using Application.Business.UserAssigns.Commands.CreateUserAssign;
using FluentValidation;

namespace Application.Business.UserBranches.Commands.CreateUserBranch
{
    public class CreateUserBranchCommandValidation : AbstractValidator<CreateUserBranchCommand>
    {
        public CreateUserBranchCommandValidation()
        {
            RuleFor(x => x.Name).NotNull();

        }
    }
}

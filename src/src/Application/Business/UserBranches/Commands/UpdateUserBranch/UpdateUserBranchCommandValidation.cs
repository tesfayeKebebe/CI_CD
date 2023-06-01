using Application.Business.UserBranches.Commands.UpdateUserBranch;
using FluentValidation;

namespace Application.Business.UserAssigns.Commands.UpdateUserAssign
{
    public class UpdateUserBranchCommandValidation : AbstractValidator<UpdateUserBranchCommand>
    {
        public UpdateUserBranchCommandValidation()
        {
            RuleFor(x => x.Name).NotNull();

        }
    }
}

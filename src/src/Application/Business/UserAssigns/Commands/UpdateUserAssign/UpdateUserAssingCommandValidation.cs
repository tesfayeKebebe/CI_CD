using FluentValidation;

namespace Application.Business.UserAssigns.Commands.UpdateUserAssign
{
    public class UpdateUserAssignCommandValidation : AbstractValidator<UpdateUserAssignCommand>
    {
        public UpdateUserAssignCommandValidation()
        {
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.UserBranchId).NotNull();

        }
    }
}

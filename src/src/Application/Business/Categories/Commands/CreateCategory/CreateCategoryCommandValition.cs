using FluentValidation;
namespace Application.Business.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValition : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValition()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}

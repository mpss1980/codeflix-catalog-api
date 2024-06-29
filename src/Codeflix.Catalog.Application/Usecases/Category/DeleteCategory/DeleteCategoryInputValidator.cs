

using FluentValidation;

namespace Codeflix.Catalog.Application.Usecases.Category.DeleteCategory;

public class DeleteCategoryInputValidator : AbstractValidator<DeleteCategoryInput>
{
    public DeleteCategoryInputValidator()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        RuleFor(x => x.Id).NotEmpty();
    }
}
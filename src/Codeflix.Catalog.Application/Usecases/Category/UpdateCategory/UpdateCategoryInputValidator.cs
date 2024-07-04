using FluentValidation;

namespace Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;

public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        RuleFor(x => x.Id).NotEmpty();
    }
}
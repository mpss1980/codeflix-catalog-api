using FluentValidation;

namespace Codeflix.Catalog.Application.Usecases.Category.GetCategory;

public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidator()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        RuleFor(x => x.Id).NotEmpty();
    }
}
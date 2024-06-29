using Codeflix.Catalog.Application.Usecases.Category.Commons;
using Codeflix.Catalog.Domain.Repositories;

namespace Codeflix.Catalog.Application.Usecases.Category.GetCategory;

public class GetCategoryUsecase : IGetCategoryUsecase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryUsecase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        return CategoryOutput.FromCategory(category);
    }
}
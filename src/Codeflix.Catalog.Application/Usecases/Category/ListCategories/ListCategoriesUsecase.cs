using Codeflix.Catalog.Application.Usecases.Category.Commons;
using Codeflix.Catalog.Domain.Params;
using Codeflix.Catalog.Domain.Repositories;

namespace Codeflix.Catalog.Application.Usecases.Category.ListCategories;

public class ListCategoriesUsecase : IListCategoriesUsecase
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategoriesUsecase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await _categoryRepository.Search(
            new SearchInput(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );

        return new ListCategoriesOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items.Select(CategoryOutput.FromCategory).ToList()
        );
    }
}
using Codeflix.Catalog.Application.Commons;
using Codeflix.Catalog.Application.Usecases.Category.Commons;

namespace Codeflix.Catalog.Application.Usecases.Category.ListCategories;

public class ListCategoriesOutput : PaginatedListOutput<CategoryOutput>
{
    public ListCategoriesOutput(int page, int perPage, int total, IReadOnlyList<CategoryOutput> items) 
        : base(page, perPage, total, items)
    {
    }
}
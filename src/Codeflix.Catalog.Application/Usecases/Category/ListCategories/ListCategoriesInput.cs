using Codeflix.Catalog.Application.Commons;
using Codeflix.Catalog.Domain.Params;
using MediatR;

namespace Codeflix.Catalog.Application.Usecases.Category.ListCategories;

public class ListCategoriesInput : PaginatedListInput, IRequest<ListCategoriesOutput>
{
    public ListCategoriesInput(int page, int perPage, string search, string sort, SearchOrder dir)
        : base(page, perPage, search, sort, dir)
    {
    }
}
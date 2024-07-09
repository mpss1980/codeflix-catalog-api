using MediatR;

namespace Codeflix.Catalog.Application.Usecases.Category.ListCategories;

public interface IListCategoriesUsecase : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{
    
}
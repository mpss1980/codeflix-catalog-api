using Codeflix.Catalog.Application.Usecases.Category.Commons;
using MediatR;

namespace Codeflix.Catalog.Application.Usecases.Category.GetCategory;

public interface IGetCategoryUsecase : IRequestHandler<GetCategoryInput, CategoryOutput>
{
   
}
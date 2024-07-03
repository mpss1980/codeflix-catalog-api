using Codeflix.Catalog.Application.Usecases.Category.Commons;
using MediatR;

namespace Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;

public interface IUpdateCategoryUsecase : IRequestHandler<UpdateCategoryInput, CategoryOutput>
{
    
}
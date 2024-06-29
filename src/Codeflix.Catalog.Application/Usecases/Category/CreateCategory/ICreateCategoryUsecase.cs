using Codeflix.Catalog.Application.Usecases.Category.Commons;
using MediatR;

namespace Codeflix.Catalog.Application.Usecases.Category.CreateCategory;

public interface ICreateCategoryUsecase : IRequestHandler<CreateCategoryInput, CategoryOutput>
{
}
using Codeflix.Catalog.Application.Usecases.Category.Commons;
using MediatR;

namespace Codeflix.Catalog.Application.Usecases.Category.GetCategory;

public class GetCategoryInput : IRequest<CategoryOutput>
{
    public Guid Id { get; set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }
}
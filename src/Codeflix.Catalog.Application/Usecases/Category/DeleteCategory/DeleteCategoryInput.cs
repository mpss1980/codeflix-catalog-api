using MediatR;

namespace Codeflix.Catalog.Application.Usecases.Category.DeleteCategory;

public class DeleteCategoryInput : IRequest
{
    public Guid Id { get; set; }

    public DeleteCategoryInput(Guid id) => Id = id;
}
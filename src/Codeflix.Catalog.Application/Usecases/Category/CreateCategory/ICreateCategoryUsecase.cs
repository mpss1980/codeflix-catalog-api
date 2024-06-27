namespace Codeflix.Catalog.Application.Usecases.Category.CreateCategory;

public interface ICreateCategoryUsecase
{
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Domain.Repositories;

namespace Codeflix.Catalog.Application.Usecases.Category.DeleteCategory;

public class DeleteCategoryUsecase : IDeleteCategoryUsecase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryUsecase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        await _categoryRepository.Delete(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
    }
}
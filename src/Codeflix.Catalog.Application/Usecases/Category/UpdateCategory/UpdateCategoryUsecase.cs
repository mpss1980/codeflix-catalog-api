using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.Usecases.Category.Commons;
using Codeflix.Catalog.Domain.Repositories;

namespace Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;

public class UpdateCategoryUsecase : IUpdateCategoryUsecase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryUsecase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        category.Update(request.Name, request.Description);
        if (request.IsActive != null && request.IsActive != category.IsActive)
            if(category.IsActive) category.Deactivate();
            else category.Activate();

        await _categoryRepository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryOutput.FromCategory(category);
    }
}
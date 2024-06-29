using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.Usecases.Category.Commons;
using Codeflix.Catalog.Domain.Repositories;
using DomainEntity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Application.Usecases.Category.CreateCategory;

public class CreateCategoryUsecase : ICreateCategoryUsecase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryUsecase(ICategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = repository;
    }

    public async Task<CategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(
            input.Name,
            input.Description,
            input.IsActive
        );

        await _categoryRepository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryOutput.FromCategory(category);
    }
}
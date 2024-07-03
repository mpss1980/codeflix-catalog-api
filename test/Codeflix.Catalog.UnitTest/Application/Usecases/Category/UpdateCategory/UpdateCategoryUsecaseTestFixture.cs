using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;
using CategoryDomain = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryUsecaseTestFixture))]
public class UpdateCategoryUsecaseTestFixtureCollection : ICollectionFixture<UpdateCategoryUsecaseTestFixture>
{
    
}

public class UpdateCategoryUsecaseTestFixture : BaseFixture
{
    private string GetValidName() => Faker.Random.String2(3, 255);

    private string GetValidDescription() => Faker.Random.String2(1, 10000);

    private bool GetValidIsActive() => Faker.Random.Bool();

    public CategoryDomain.Category CreateCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );
    
    public UpdateCategoryInput GetValidInput(Guid? id = null) => new(
        id ?? Guid.NewGuid(),
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );
    
    public static Mock<ICategoryRepository> GetCategoryRepositoryMock() => new();
    
    public static Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}
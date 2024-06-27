using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.Usecases.Category;
using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryUsecaseTestFixture))]
public class CreateCategoryUsecaseTestFixtureCollection : ICollectionFixture<CreateCategoryUsecaseTestFixture>
{
}

public class CreateCategoryUsecaseTestFixture : BaseFixture
{
    public string GetValidName() => Faker.Random.String2(3, 255);

    public string GetValidDescription() => Faker.Random.String2(1, 10000);

    public bool GetValidIsActive() => Faker.Random.Bool();

    public CreateCategoryInput CreateCategoryInput() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );

    public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();
}
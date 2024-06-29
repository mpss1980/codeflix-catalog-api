using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;
using CategoryDomain = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryUsecaseTestFixture))]
public class GetCategoryUsecaseTestFixtureCollection : ICollectionFixture<GetCategoryUsecaseTestFixture>
{
}

public class GetCategoryUsecaseTestFixture : BaseFixture
{
    private string GetValidName() => Faker.Random.String2(3, 255);

    private string GetValidDescription() => Faker.Random.String2(1, 10000);

    private bool GetValidIsActive() => Faker.Random.Bool();

    public CategoryDomain.Category GetValidCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );
    
    public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();
}
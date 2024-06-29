using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryUsecaseTestFixture))]
public class DeleteCategoryUsecaseTestFixtureCollection : ICollectionFixture<DeleteCategoryUsecaseTestFixture>
{
    
}

public class DeleteCategoryUsecaseTestFixture : BaseFixture
{
    private string GetValidName() => Faker.Random.String2(3, 255);

    private string GetValidDescription() => Faker.Random.String2(1, 10000);

    private bool GetValidIsActive() => Faker.Random.Bool();

    public Catalog.Domain.Entities.Category GetValidCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );
    
    public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();
}
using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.Infra.Data.EntityFramework;
using Codeflix.Catalog.IntegrationTest.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Repositories.CategoryRepository;

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture>
{
}

public class CategoryRepositoryTestFixture : BaseFixture
{
    private string GetValidName() => Faker.Random.String2(3, 255);

    private string GetValidDescription() => Faker.Random.String2(1, 10000);

    private bool GetValidIsActive() => Faker.Random.Bool();

    public Category GetValidCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );

    public List<Category> GetCategoryList(int length = 10) =>
        Enumerable.Range(0, length).Select(_ => GetValidCategory()).ToList();

    public CodeflixCategoryDbContext CreateDbContext()
    {
        var dbContext = new CodeflixCategoryDbContext(
            new DbContextOptionsBuilder<CodeflixCategoryDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );
        return dbContext;
    }
}
using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.Domain.Params;
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

    public List<Category> GetCategoryListWithNames(List<string> names) =>
        names.Select(name =>
        {
            var category = GetValidCategory();
            category.Update(name);
            return category;
        }).ToList();

    public List<Category> CloneCategoriesListOrdered(List<Category> categories, string orderBy, SearchOrder order)
    {
        var listClone = new List<Category>(categories);
        var enumerableOrdered = (orderBy, order) switch
        {
            ("name", SearchOrder.Ascending) => listClone.OrderBy(x => x.Name),
            ("name", SearchOrder.Descending) => listClone.OrderByDescending(x => x.Name),
            ("id", SearchOrder.Ascending) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Descending) => listClone.OrderByDescending(x => x.Id),
            ("createdAt", SearchOrder.Ascending) => listClone.OrderBy(x => x.CreatedAt),
            ("createdAt", SearchOrder.Descending) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name)
        };
        return enumerableOrdered.ToList();
    }

    public CodeflixCategoryDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new CodeflixCategoryDbContext(
            new DbContextOptionsBuilder<CodeflixCategoryDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );
        if (!preserveData) dbContext.Database.EnsureDeleted();
        return dbContext;
    }
}
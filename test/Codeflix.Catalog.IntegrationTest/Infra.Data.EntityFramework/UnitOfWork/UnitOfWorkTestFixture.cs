using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.IntegrationTest.Common;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection : ICollectionFixture<UnitOfWorkTestFixture>
{
}


public class UnitOfWorkTestFixture : BaseFixture
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
}
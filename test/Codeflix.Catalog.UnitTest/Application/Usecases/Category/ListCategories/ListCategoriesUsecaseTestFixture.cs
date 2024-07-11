using Codeflix.Catalog.Application.Usecases.Category.ListCategories;
using Codeflix.Catalog.Domain.Params;
using Codeflix.Catalog.UnitTest.Application.Usecases.Category.Common;
using Xunit;
using CategoryDomain = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesUsecaseTestFixture))]
public class ListCategoriesUsecaseTestFixtureCollection : ICollectionFixture<ListCategoriesUsecaseTestFixture>
{
}

public class ListCategoriesUsecaseTestFixture : CategoryUsecasesBaseFixture
{
    private CategoryDomain.Category CreateCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );

    public ListCategoriesInput GetListCategoriesInput()
    {
        return new ListCategoriesInput(
            page: Faker.Random.Int(1, 10),
            perPage: Faker.Random.Int(1, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: Faker.Random.Int(0, 10) > 5 ? SearchOrder.Ascending : SearchOrder.Descending
        );
    }

    public List<CategoryDomain.Category> CreateCategories(int count) =>
        Enumerable.Range(1, count).Select(_ => CreateCategory()).ToList();
}
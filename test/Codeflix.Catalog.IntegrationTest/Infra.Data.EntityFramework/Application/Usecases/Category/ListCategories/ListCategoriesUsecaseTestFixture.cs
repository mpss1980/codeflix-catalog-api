using Codeflix.Catalog.Domain.Params;
using Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesUsecaseTestFixture))]
public class ListCategoriesUsecaseTestFixtureCollection : ICollectionFixture<ListCategoriesUsecaseTestFixture>
{
}

public class ListCategoriesUsecaseTestFixture : CategoryUseCasesBaseFixture
{
    public List<Domain.Entities.Category> GetCategoriesListWithNames(
        List<string> names
    ) => names.Select(name =>
    {
        var category = GetValidCategory();
        category.Update(name);
        return category;
    }).ToList();

    public List<Domain.Entities.Category> CloneCategoriesListOrdered(
        List<Domain.Entities.Category> categories,
        string orderedBy,
        SearchOrder order
    )
    {
        var clonedList = new List<Domain.Entities.Category>(categories);
        var orderedEnumerable = (orderedBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Ascending) => clonedList.OrderBy(c => c.Name),
            ("name", SearchOrder.Descending) => clonedList.OrderByDescending(c => c.Name),
            ("id", SearchOrder.Ascending) => clonedList.OrderBy(c => c.Id),
            ("id", SearchOrder.Descending) => clonedList.OrderByDescending(c => c.Id),
            ("CreatedAt", SearchOrder.Ascending) => clonedList.OrderBy(c => c.CreatedAt),
            ("CreatedAt", SearchOrder.Descending) => clonedList.OrderByDescending(c => c.CreatedAt),
            _ => clonedList.OrderBy(c => c.Name)
        };
        return orderedEnumerable.ToList();
    }
}
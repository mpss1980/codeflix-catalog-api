using Codeflix.Catalog.UnitTest.Application.Usecases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryUsecaseTestFixture))]
public class GetCategoryUsecaseTestFixtureCollection : ICollectionFixture<GetCategoryUsecaseTestFixture>
{
}

public class GetCategoryUsecaseTestFixture : CategoryUsecasesBaseFixture
{
}
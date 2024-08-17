using Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryUsecaseTestFixture))]
public class GetCategoryUsecaseTestCollection : ICollectionFixture<GetCategoryUsecaseTestFixture>
{
}

public class GetCategoryUsecaseTestFixture : CategoryUseCasesBaseFixture
{
    
}
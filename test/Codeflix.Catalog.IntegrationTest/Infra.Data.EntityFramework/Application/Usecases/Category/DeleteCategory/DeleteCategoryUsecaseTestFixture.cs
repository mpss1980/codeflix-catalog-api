using Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryUsecaseTestFixture))]
public class DeleteCategoryUsecaseTestFixtureCollection : ICollectionFixture<DeleteCategoryUsecaseTestFixture>
{
}
public class DeleteCategoryUsecaseTestFixture : CategoryUseCasesBaseFixture
{
    
}
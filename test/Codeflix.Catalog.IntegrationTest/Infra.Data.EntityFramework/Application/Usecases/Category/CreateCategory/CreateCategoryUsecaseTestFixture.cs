using Codeflix.Catalog.Application.Usecases.Category.CreateCategory;
using Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryUsecaseTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryUsecaseTestFixture>
{
}
public class CreateCategoryUsecaseTestFixture : CategoryUseCasesBaseFixture
{
    
    public CreateCategoryInput GetValidInput()
    {
        var category = GetValidCategory();
        return new CreateCategoryInput(
            category.Name,
            category.Description,
            category.IsActive
        );
    }
    
    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var category = GetValidInput();
        return new CreateCategoryInput(
            new string('a', 300),
            category.Description,
            category.IsActive
        );
    }

    public CreateCategoryInput GetInvalidInputCategoryNull()
    {
        var input = GetValidInput();
        input.Description = null!;
        return input;
    }
    
    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        var input = GetValidInput();
        input.Description = new string('a', 10001)!;
        return input;
    }

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var input = GetValidInput();
        var substring = input.Name.Substring(0, 2);
        input.Name = substring;
        return input;
    }
}
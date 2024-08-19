using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;
using Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryUsecaseTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryUsecaseTestFixture>
{
}

public class UpdateCategoryUsecaseTestFixture : CategoryUseCasesBaseFixture
{
    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new(
            id ?? Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            GetValidIsActive()
        );
    }

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var input = GetValidInput();
        input.Name = Faker.Random.String2(0, 2);
        return input;
    }
    
    public UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var input = GetValidInput();
        input.Name = new string('a', 300);
        return input;
    }
    
    public UpdateCategoryInput GetInvalidInputTooLongDescription()
    {
        var input = GetValidInput();
        input.Description = new string('a', 10001)!;
        return input;
    }
    
}
using Codeflix.Catalog.Application.Usecases.Category.CreateCategory;
using Codeflix.Catalog.UnitTest.Application.Usecases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryUsecaseTestFixture))]
public class CreateCategoryUsecaseTestFixtureCollection : ICollectionFixture<CreateCategoryUsecaseTestFixture>
{
}

public class CreateCategoryUsecaseTestFixture : CategoryUsecasesBaseFixture
{
    public CreateCategoryInput CreateCategoryInput() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = CreateCategoryInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];
        return invalidInputShortName;
    }
    
    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = CreateCategoryInput();
        invalidInputTooLongName.Name = string.Join(null, Enumerable.Range(1, 256).Select(_ => "A").ToArray());
        return invalidInputTooLongName;
    }
    
    public CreateCategoryInput GetInvalidInputNullDescription()
    {
        var invalidInputNullDescription = CreateCategoryInput();
        invalidInputNullDescription.Description = null!;
        return invalidInputNullDescription;
    }

    public CreateCategoryInput GetInvalidInputLongDescription()
    {
        var invalidInputLongDescription = CreateCategoryInput();
        invalidInputLongDescription.Description =
            string.Join(null, Enumerable.Range(1, 10001).Select(_ => "A").ToArray());
        return invalidInputLongDescription;
    }
}
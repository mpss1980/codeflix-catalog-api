using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;
using CategoryDomain = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryUsecaseTestFixture))]
public class UpdateCategoryUsecaseTestFixtureCollection : ICollectionFixture<UpdateCategoryUsecaseTestFixture>
{
    
}

public class UpdateCategoryUsecaseTestFixture : BaseFixture
{
    private string GetValidName() => Faker.Random.String2(3, 255);

    private string GetValidDescription() => Faker.Random.String2(1, 10000);

    private bool GetValidIsActive() => Faker.Random.Bool();

    public CategoryDomain.Category CreateCategory() => new(
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );
    
    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];
        return invalidInputShortName;
    }
    
    public UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetValidInput();
        invalidInputTooLongName.Name = string.Join(null, Enumerable.Range(1, 256).Select(_ => "A").ToArray());
        return invalidInputTooLongName;
    }
    
    public UpdateCategoryInput GetInvalidInputLongDescription()
    {
        var invalidInputLongDescription = GetValidInput();
        invalidInputLongDescription.Description =
            string.Join(null, Enumerable.Range(1, 10001).Select(_ => "A").ToArray());
        return invalidInputLongDescription;
    }
    
    public UpdateCategoryInput GetValidInput(Guid? id = null) => new(
        id ?? Guid.NewGuid(),
        GetValidName(),
        GetValidDescription(),
        GetValidIsActive()
    );
    
    public static Mock<ICategoryRepository> GetCategoryRepositoryMock() => new();
    
    public static Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}
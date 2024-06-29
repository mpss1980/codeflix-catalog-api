using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.Usecases.Category;
using Codeflix.Catalog.Application.Usecases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryUsecaseTestFixture))]
public class CreateCategoryUsecaseTestFixtureCollection : ICollectionFixture<CreateCategoryUsecaseTestFixture>
{
}

public class CreateCategoryUsecaseTestFixture : BaseFixture
{
    public string GetValidName() => Faker.Random.String2(3, 255);

    public string GetValidDescription() => Faker.Random.String2(1, 10000);

    private bool GetValidIsActive() => Faker.Random.Bool();

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

    public static Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();
    
    public static Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();
}
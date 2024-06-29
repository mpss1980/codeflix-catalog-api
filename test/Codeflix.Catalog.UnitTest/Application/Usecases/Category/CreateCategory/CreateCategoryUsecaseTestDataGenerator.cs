namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.CreateCategory;

public class CreateCategoryUsecaseTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryUsecaseTestFixture();
        var invalidInputList = new List<object[]>
        {
            new object[] { fixture.GetInvalidInputShortName() },
            new object[] { fixture.GetInvalidInputTooLongName() },
            new object[] { fixture.GetInvalidInputNullDescription() },
            new object[] { fixture.GetInvalidInputLongDescription() }
        };

        return invalidInputList;
    }
}
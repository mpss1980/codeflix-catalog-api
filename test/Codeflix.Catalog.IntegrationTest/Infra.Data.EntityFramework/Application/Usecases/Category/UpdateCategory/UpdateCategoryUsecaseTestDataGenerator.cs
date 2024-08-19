namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.UpdateCategory;

public class UpdateCategoryUsecaseTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryUsecaseTestFixture();
        for (var i = 0; i < times; i++)
        {
            var category = fixture.GetValidCategory();
            var input = fixture.GetValidInput(category.Id);
            yield return new object[]
            {
                category, input
            };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new UpdateCategoryUsecaseTestFixture();
        var invalidInputList = new List<object[]>
        {
            new object[] { fixture.GetInvalidInputShortName() },
            new object[] { fixture.GetInvalidInputTooLongName() },
            new object[] { fixture.GetInvalidInputTooLongDescription() }
        };

        return invalidInputList;
    }
}
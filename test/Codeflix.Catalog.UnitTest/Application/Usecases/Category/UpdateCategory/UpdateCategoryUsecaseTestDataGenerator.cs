using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.UpdateCategory;

public class UpdateCategoryUsecaseTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoryToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryUsecaseTestFixture();
        for (var index = 0; index < times; index++)
        {
            var category = fixture.CreateCategory();
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
            new object[]
            {
                fixture.GetInvalidInputShortName(), "Name should be at least 3 characters long"
            },
            new object[]
            {
                fixture.GetInvalidInputTooLongName(), "Name should be less or equal to 255 characters long"
            },
            new object[]
            {
                fixture.GetInvalidInputLongDescription(), "Description should be less or equal to 10000 characters long"
            }
        };

        return invalidInputList;
    }
}
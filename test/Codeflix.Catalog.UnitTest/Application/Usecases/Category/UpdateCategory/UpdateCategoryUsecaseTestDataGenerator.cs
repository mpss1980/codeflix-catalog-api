using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.UpdateCategory;

public class UpdateCategoryUsecaseTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoryToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryUsecaseTestFixture();
        for(var index = 0; index < times; index++)
        {
            var category = fixture.CreateCategory();
            var input = fixture.GetValidInput(category.Id);
            yield return new object[]
            {
                category, input
            };
        }
    }
}
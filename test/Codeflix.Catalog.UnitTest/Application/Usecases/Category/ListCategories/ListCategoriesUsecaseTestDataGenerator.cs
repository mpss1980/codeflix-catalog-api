using Codeflix.Catalog.Application.Usecases.Category.ListCategories;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.ListCategories;

public class ListCategoriesUsecaseTestDataGenerator
{
    public static IEnumerable<object[]> GetInputsWithoutAllParameters()
    {
        var fixture = new ListCategoriesUsecaseTestFixture();
        var input = fixture.GetListCategoriesInput();
        var inputs = new List<object[]>
        {
            new object[] { new ListCategoriesInput() },
            new object[] { new ListCategoriesInput(input.Page) },
            new object[] { new ListCategoriesInput(input.Page, input.PerPage) },
            new object[] { new ListCategoriesInput(input.Page, input.PerPage, input.Search) },
            new object[] { new ListCategoriesInput(input.Page, input.PerPage, input.Search, input.Sort) },
            new object[] { new ListCategoriesInput(input.Page, input.PerPage, input.Search, input.Sort, input.Dir) }
        };
        return inputs;
    }
}
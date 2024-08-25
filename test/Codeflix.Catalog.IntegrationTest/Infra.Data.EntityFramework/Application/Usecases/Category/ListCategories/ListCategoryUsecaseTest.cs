using Codeflix.Catalog.Application.Usecases.Category.Commons;
using Codeflix.Catalog.Application.Usecases.Category.ListCategories;
using Codeflix.Catalog.Domain.Params;
using Codeflix.Catalog.Infra.Data.EntityFramework.Repositories;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.ListCategories;

[Collection(nameof(ListCategoriesUsecaseTestFixture))]
public class ListCategoryUsecaseTest
{
    private readonly ListCategoriesUsecaseTestFixture _fixture;

    public ListCategoryUsecaseTest(ListCategoriesUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SearchReturnAListAndTotal))]
    public async Task SearchReturnAListAndTotal()
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList(10);
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(1, 20);
        var usecase = new ListCategoriesUsecase(repository);

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);

        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(category => category.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnEmptyWhenEmpty))]
    public async Task SearchReturnEmptyWhenEmpty()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(1, 20);
        var usecase = new ListCategoriesUsecase(repository);

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchReturnsPaginatedList))]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPaginatedList(
        int quantityCategoriesToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList(quantityCategoriesToGenerate);
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(page, perPage);
        var usecase = new ListCategoriesUsecase(repository);

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(expectedQuantityItems);
        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(category => category.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(
        string searchText,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems
    )
    {
        var categoryNamesList = new List<string>()
        {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };

        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoriesListWithNames(categoryNamesList);
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(page, perPage, searchText);
        var usecase = new ListCategoriesUsecase(repository);

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(category => category.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchOrdered))]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("CreatedAt", "asc")]
    [InlineData("CreatedAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdered(
        string orderedBy,
        string order
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList(10);
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new CategoryRepository(dbContext);
        var usecaseOrder = order == "asc" ? SearchOrder.Ascending : SearchOrder.Descending;
        var input = new ListCategoriesInput(1, 20, "", orderedBy, usecaseOrder);
        var usecase = new ListCategoriesUsecase(repository);

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);

        var expectedList = _fixture.CloneCategoriesListOrdered(categories, input.Sort, input.Dir);
        for (var i = 0; i < expectedList.Count; i++)
        {
            var outputItem = output.Items[i];
            var item = expectedList[i];
            outputItem.Should().NotBeNull();
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item.Name);
            outputItem.Id.Should().Be(item.Id);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }
}
using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Domain.Params;
using FluentAssertions;
using Xunit;
using Repository = Codeflix.Catalog.Infra.Data.EntityFramework.Repositories;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    public async Task Insert()
    {
        var dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var repository = new Repository.CategoryRepository(dbContext);

        await repository.Insert(category, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(category.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(category.Name);
        dbCategory.Description.Should().Be(category.Description);
        dbCategory.IsActive.Should().Be(category.IsActive);
        dbCategory.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    public async Task Get()
    {
        var dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var categories = _fixture.GetCategoryList();
        categories.Add(category);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var repository = new Repository.CategoryRepository(dbContext);

        var dbCategory = await repository.Get(category.Id, CancellationToken.None);

        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(category.Name);
        dbCategory.Description.Should().Be(category.Description);
        dbCategory.IsActive.Should().Be(category.IsActive);
        dbCategory.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    public async Task GetThrowIfNotFound()
    {
        var id = Guid.NewGuid();
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList();

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var repository = new Repository.CategoryRepository(dbContext);

        var task = async () => await repository.Get(id, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {id} not found.");
    }

    [Fact(DisplayName = nameof(Update))]
    public async Task Update()
    {
        var dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var updatedCategory = _fixture.GetValidCategory();
        var categories = _fixture.GetCategoryList();
        categories.Add(category);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        category.Update(updatedCategory.Name, updatedCategory.Description);

        var repository = new Repository.CategoryRepository(dbContext);
        await repository.Update(category, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(category.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(updatedCategory.Name);
        dbCategory.Description.Should().Be(updatedCategory.Description);
        dbCategory.IsActive.Should().Be(category.IsActive);
        dbCategory.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(Delete))]
    public async Task Delete()
    {
        var dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var categories = _fixture.GetCategoryList();
        categories.Add(category);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new Repository.CategoryRepository(dbContext);
        await repository.Delete(category, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(category.Id);
        dbCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
    public async Task SearchReturnsListAndTotal()
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList();
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Ascending);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new Repository.CategoryRepository(dbContext);
        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);

        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(x => x.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
    public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
    {
        var dbContext = _fixture.CreateDbContext();
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Ascending);

        var repository = new Repository.CategoryRepository(dbContext);
        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 1, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    public async Task SearchReturnsPaginated(
        int categoryQuantityToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList(categoryQuantityToGenerate);
        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Ascending);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new Repository.CategoryRepository(dbContext);
        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(x => x.Id == outputItem.Id);
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
    [InlineData("Sci-Fi", 1, 5, 4, 4)]
    [InlineData("Sci-Fi", 1, 2, 2, 4)]
    [InlineData("Sci-Fi", 2, 3, 1, 4)]
    [InlineData("Sci-Fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(
        string searchText,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityItemsTotal
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryListWithNames(new List<string>()
        {
            "Action",
            "Horror",
            "Horror Robots",
            "Horror - Based on Real Stories",
            "Drama",
            "Sci-Fi IA",
            "Sci-Fi Robots",
            "Sci-Fi Space",
            "Sci-Fi Future",
        });
        var searchInput = new SearchInput(page, perPage, searchText, "", SearchOrder.Ascending);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new Repository.CategoryRepository(dbContext);
        var output = await repository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedQuantityItemsTotal);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);

        foreach (var outputItem in output.Items)
        {
            var item = categories.Find(x => x.Id == outputItem.Id);
            item.Should().NotBeNull();
            outputItem.Name.Should().Be(item.Name);
            outputItem.Description.Should().Be(item.Description);
            outputItem.IsActive.Should().Be(item.IsActive);
            outputItem.CreatedAt.Should().Be(item.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(OrderedSearch))]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task OrderedSearch(
        string orderBy,
        string order
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList(10);
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Ascending : SearchOrder.Descending;
        var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new Repository.CategoryRepository(dbContext);
        var output = await repository.Search(searchInput, CancellationToken.None);

        var expectedOrderedList = _fixture.CloneCategoriesListOrdered(
            categories,
            orderBy,
            searchOrder
        );
        
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);
        for (var index = 0; index < output.Items.Count; index++)
        {
            var outputItem = output.Items[index];
            var expectedItem = expectedOrderedList[index];
            expectedItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.Name.Should().Be(expectedItem.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }
}
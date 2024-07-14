using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Infra.Data.EntityFramework;
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

        var dbCategory = await dbContext.Categories.FindAsync(category.Id);
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
}
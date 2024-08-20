using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Usecases.Category.DeleteCategory;
using Codeflix.Catalog.Infra.Data.EntityFramework.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryUsecaseTestFixture))]
public class DeleteCategoryUsecaseTest
{
    private readonly DeleteCategoryUsecaseTestFixture _fixture;

    public DeleteCategoryUsecaseTest(DeleteCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    public async Task DeleteCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var categories = _fixture.GetCategoryList();

        await dbContext.AddRangeAsync(categories);
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new DeleteCategoryUsecase(repository, unitOfWork);

        var input = new DeleteCategoryInput(category.Id);
        await usecase.Handle(input, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var dbCategoryDeleted = await assertDbContext.Categories.FindAsync(category.Id);
        dbCategoryDeleted.Should().BeNull();

        var dbCategories = await assertDbContext.Categories.ToListAsync();
        dbCategories.Should().HaveCount(categories.Count);
    }

    [Fact(DisplayName = nameof(DeleteCategoryAndThrowExceptionWhenNotFound))]
    public async Task DeleteCategoryAndThrowExceptionWhenNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList(10);
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new DeleteCategoryUsecase(repository, unitOfWork);

        var input = new DeleteCategoryInput(Guid.NewGuid());

        var task = async () => await usecase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {input.Id} not found.");
    }
}
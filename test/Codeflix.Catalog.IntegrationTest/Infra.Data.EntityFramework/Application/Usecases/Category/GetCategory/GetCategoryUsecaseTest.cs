using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Usecases.Category.GetCategory;
using Codeflix.Catalog.Infra.Data.EntityFramework.Repositories;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.GetCategory;

[Collection(nameof(GetCategoryUsecaseTestFixture))]
public class GetCategoryUsecaseTest
{
    private readonly GetCategoryUsecaseTestFixture _fixture;

    public GetCategoryUsecaseTest(GetCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    public async Task GetCategory()
    {
        var category = _fixture.GetValidCategory();
        var dbContext = _fixture.CreateDbContext();
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var input = new GetCategoryInput(category.Id);
        var usecase = new GetCategoryUsecase(repository);

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesNotExist))]
    public async Task NotFoundExceptionWhenCategoryDoesNotExist()
    {
        var category = _fixture.GetValidCategory();
        var dbContext = _fixture.CreateDbContext();
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var input = new GetCategoryInput(Guid.NewGuid());
        var usecase = new GetCategoryUsecase(repository);

        var task = async () => await usecase.Handle(input, CancellationToken.None);
        
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {input.Id} not found.");
    }
}
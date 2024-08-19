using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Infra.Data.EntityFramework.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryUsecaseTestFixture))]
public class UpdateCategoryUsecaseTest
{
    private readonly UpdateCategoryUsecaseTestFixture _fixture;

    public UpdateCategoryUsecaseTest(UpdateCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCategory))]
    [MemberData(
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator)
    )]
    public async Task UpdateCategory(
        Domain.Entities.Category category,
        UpdateCategoryInput input
    )
    {
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCategoryList());
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new UpdateCategoryUsecase(repository, unitOfWork);

        var output = await usecase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithOutIsActive))]
    [MemberData(
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator)
    )]
    public async Task UpdateCategoryWithOutIsActive(
        Domain.Entities.Category category,
        UpdateCategoryInput input
    )
    {
        var inputToUpdate = new UpdateCategoryInput(
            input.Id,
            input.Name,
            input.Description
        );

        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCategoryList());
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new UpdateCategoryUsecase(repository, unitOfWork);

        var output = await usecase.Handle(inputToUpdate, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(inputToUpdate.Name);
        dbCategory.Description.Should().Be(inputToUpdate.Description);
        dbCategory.IsActive.Should().Be(category.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(inputToUpdate.Name);
        output.Description.Should().Be(inputToUpdate.Description);
        output.IsActive.Should().Be(category.IsActive);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithOutIsActive))]
    [MemberData(
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator)
    )]
    public async Task UpdateCategoryOnlyName(
        Domain.Entities.Category category,
        UpdateCategoryInput input
    )
    {
        var inputToUpdate = new UpdateCategoryInput(
            input.Id,
            input.Name
        );

        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCategoryList());
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new UpdateCategoryUsecase(repository, unitOfWork);

        var output = await usecase.Handle(inputToUpdate, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(inputToUpdate.Name);
        dbCategory.Description.Should().Be(category.Description);
        dbCategory.IsActive.Should().Be(category.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(inputToUpdate.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundCategory))]
    public async Task UpdateThrowsWhenNotFoundCategory()
    {
        var inputToUpdate = _fixture.GetValidInput();
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCategoryList());
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new UpdateCategoryUsecase(repository, unitOfWork);

        var task = async () => await usecase.Handle(inputToUpdate, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {inputToUpdate.Id} not found.");
    }
    
    [Theory(DisplayName = nameof(UpdateThrowsWhenCantInstantiateCategory))]
    [MemberData(
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator)
    )]
    public async Task UpdateThrowsWhenCantInstantiateCategory(
        UpdateCategoryInput input
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetCategoryList();
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new UpdateCategoryUsecase(repository, unitOfWork);
        input.Id = categories[0].Id;

        var task = async () => await usecase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>();
    }
}
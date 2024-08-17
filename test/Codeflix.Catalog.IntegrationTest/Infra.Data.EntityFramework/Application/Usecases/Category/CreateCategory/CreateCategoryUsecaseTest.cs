using Codeflix.Catalog.Application.Usecases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Infra.Data.EntityFramework.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.Application.Usecases.Category.CreateCategory;

[Collection(nameof(CreateCategoryUsecaseTestFixture))]
public class CreateCategoryUsecaseTest
{
    private readonly CreateCategoryUsecaseTestFixture _fixture;

    public CreateCategoryUsecaseTest(CreateCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    public async Task CreateCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new CreateCategoryUsecase(repository, unitOfWork);

        var input = _fixture.GetValidInput();
        var output = await usecase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithName))]
    public async Task CreateCategoryOnlyWithName()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new CreateCategoryUsecase(repository, unitOfWork);

        var input = new CreateCategoryInput(_fixture.GetValidCategory().Name, null, true);
        var output = await usecase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithNameAndDescription))]
    public async Task CreateCategoryOnlyWithNameAndDescription()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new CreateCategoryUsecase(repository, unitOfWork);

        var input = new CreateCategoryInput(_fixture.GetValidCategory().Name, _fixture.GetValidCategory().Description,
            true);
        var output = await usecase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowExceptionWhenCannotInstantiateCategory))]
    [MemberData(
        nameof(CreateCategoryUsecaseTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateCategoryUsecaseTestDataGenerator)
    )]
    public async Task ThrowExceptionWhenCannotInstantiateCategory(
        CreateCategoryInput input)
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new Catalog.Infra.Data.EntityFramework.UnitOfWork(dbContext);
        var usecase = new CreateCategoryUsecase(repository, unitOfWork);

        var task = async () => await usecase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>();
        var dbCategoryList = _fixture.CreateDbContext(true).Categories.AsNoTracking().ToList();
        dbCategoryList.Should().HaveCount(0);
    }
}
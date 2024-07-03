using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Usecases.Category.Commons;
using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;
using FluentAssertions;
using Moq;
using Xunit;
using CategoryDomain = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.UpdateCategory;

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
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetCategoryToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator))
    ]
    public async Task UpdateCategory(CategoryDomain.Category category, UpdateCategoryInput input)
    {
        var repositoryMock = UpdateCategoryUsecaseTestFixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = UpdateCategoryUsecaseTestFixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            category.Id,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(category);

        var usecase = new UpdateCategoryUsecase(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);

        repositoryMock.Verify(x => x.Get(
            category.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        repositoryMock.Verify(x => x.Update(
            category,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    public async Task ThrowWhenCategoryNotFound()
    {
        var repositoryMock = UpdateCategoryUsecaseTestFixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = UpdateCategoryUsecaseTestFixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();

        repositoryMock.Setup(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException($"category '{input.Id}' not found"));

        var usecase = new UpdateCategoryUsecase(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async () =>  await usecase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Usecases.Category.DeleteCategory;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryUsecaseTestFixture))]
public class DeleteCategoryUsecaseTest
{
    private readonly DeleteCategoryUsecaseTestFixture _fixture;

    public DeleteCategoryUsecaseTest(DeleteCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ShouldDeleteCategory))]
    public async Task ShouldDeleteCategory()
    {
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var validCategory = _fixture.GetValidCategory();

        categoryRepositoryMock.Setup(x => x.Get(
            validCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(validCategory);

        var input = new DeleteCategoryInput(validCategory.Id);
        var usecase = new DeleteCategoryUsecase(categoryRepositoryMock.Object, unitOfWorkMock.Object);

        await usecase.Handle(input, CancellationToken.None);

        categoryRepositoryMock.Verify(x => x.Get(
            validCategory.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        categoryRepositoryMock.Verify(x => x.Delete(
            validCategory,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(ThrowWhenCategoryIsNotFound))]
    public async Task ThrowWhenCategoryIsNotFound()
    {
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var id = Guid.NewGuid();

        categoryRepositoryMock.Setup(x => x.Get(
            id,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"Category {id} not found"));

        var input = new DeleteCategoryInput(id);
        var usecase = new DeleteCategoryUsecase(categoryRepositoryMock.Object, unitOfWorkMock.Object);

        var task = async () => await usecase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        categoryRepositoryMock.Verify(x => x.Get(
            id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
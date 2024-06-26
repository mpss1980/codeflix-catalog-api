using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.Domain.Repositories;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application;

public class CreateCategoryUsecaseTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    public async void CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var usecase = new CreateCategoryUsecase(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            "Category Name",
            "Category Description",
            true
        );

        var output = await usecase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
    }
}
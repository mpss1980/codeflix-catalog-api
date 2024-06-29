using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Usecases.Category.GetCategory;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.GetCategory;

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
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var category = _fixture.GetValidCategory();

        repositoryMock.Setup(r => r.Get(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(category);
        
        var input = new GetCategoryInput(category.Id);
        var usecase = new GetCategoryUsecase(repositoryMock.Object); 
        var output = await usecase.Handle(input, CancellationToken.None);
        
        repositoryMock.Verify(r => r.Get(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()
        ), Times.Once);
        
        output.Should().NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }
    
    [Fact(DisplayName = nameof(ThrowNotFoundExceptionWhenRepositoryThrowsException))]
    public async Task ThrowNotFoundExceptionWhenRepositoryThrowsException()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var categoryId = Guid.NewGuid();

        repositoryMock.Setup(r => r.Get(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException($"Category '{categoryId}'not found"));
        
        var input = new GetCategoryInput(categoryId);
        var usecase = new GetCategoryUsecase(repositoryMock.Object); 
        
        var task = async() => await usecase.Handle(input, CancellationToken.None);
        
        await task.Should().ThrowAsync<NotFoundException>();
        
        repositoryMock.Verify(r => r.Get(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Usecases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Exceptions;
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
        output.IsActive.Should().Be((bool) input.IsActive!);

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

    [Theory(DisplayName = nameof(UpdateCategoryWithNoIsActiveProvided))]
    [MemberData(
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetCategoryToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator))
    ]
    public async Task UpdateCategoryWithNoIsActiveProvided(CategoryDomain.Category category, UpdateCategoryInput input)
    {
        var repositoryMock = UpdateCategoryUsecaseTestFixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = UpdateCategoryUsecaseTestFixture.GetUnitOfWorkMock();
        var inputWithoutIsActive = new UpdateCategoryInput(
            input.Id,
            input.Name,
            input.Description
        );

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
        output.Id.Should().Be(inputWithoutIsActive.Id);
        output.Name.Should().Be(inputWithoutIsActive.Name);
        output.Description.Should().Be(inputWithoutIsActive.Description);

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
    
    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [MemberData(
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetCategoryToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator))
    ]
    public async Task UpdateCategoryOnlyName(CategoryDomain.Category category, UpdateCategoryInput input)
    {
        var repositoryMock = UpdateCategoryUsecaseTestFixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = UpdateCategoryUsecaseTestFixture.GetUnitOfWorkMock();
        var inputWithoutIsActive = new UpdateCategoryInput(
            input.Id,
            input.Name
        );

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
        output.Id.Should().Be(inputWithoutIsActive.Id);
        output.Name.Should().Be(inputWithoutIsActive.Name);
        output.Description.Should().Be(input.Description);

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

        var task = async () => await usecase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
    
    [Theory(DisplayName = nameof(ThrowWhenCannotUpdateCategory))]
    [MemberData(
        nameof(UpdateCategoryUsecaseTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateCategoryUsecaseTestDataGenerator)
    )]
    public async Task ThrowWhenCannotUpdateCategory(
        UpdateCategoryInput input,
        string expectedMessage
        )
    {
        var repositoryMock = UpdateCategoryUsecaseTestFixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = UpdateCategoryUsecaseTestFixture.GetUnitOfWorkMock();
        var category = _fixture.CreateCategory();
        input.Id = category.Id;
        
        repositoryMock.Setup(x => x.Get(
            category.Id,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(category);

        var usecase = new UpdateCategoryUsecase(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async () => await usecase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedMessage);
        
        repositoryMock.Verify(x => x.Get(
            category.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
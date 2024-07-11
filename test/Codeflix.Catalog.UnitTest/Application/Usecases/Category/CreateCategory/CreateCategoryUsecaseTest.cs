using Codeflix.Catalog.Application.Usecases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.CreateCategory;

[Collection(nameof(CreateCategoryUsecaseTestFixture))]
public class CreateCategoryUsecaseTest
{
    private readonly CreateCategoryUsecaseTestFixture _fixture;

    public CreateCategoryUsecaseTest(CreateCategoryUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    public async void CreateCategory()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var usecase = new CreateCategoryUsecase(repositoryMock.Object, unitOfWorkMock.Object);
        var input = _fixture.CreateCategoryInput();

        var output = await usecase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Insert(
                It.IsAny<Catalog.Domain.Entities.Category>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once
        );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithName))]
    public async void CreateCategoryOnlyWithName()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var usecase = new CreateCategoryUsecase(repositoryMock.Object, unitOfWorkMock.Object);
        var input = new CreateCategoryInput(
            _fixture.GetValidName()
        );

        var output = await usecase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Insert(
                It.IsAny<Catalog.Domain.Entities.Category>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once
        );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().BeEmpty();
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyWithNameAndDescription))]
    public async void CreateCategoryOnlyWithNameAndDescription()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var usecase = new CreateCategoryUsecase(repositoryMock.Object, unitOfWorkMock.Object);
        var input = new CreateCategoryInput(
            _fixture.GetValidName(),
            _fixture.GetValidDescription()
        );

        var output = await usecase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Insert(
                It.IsAny<Catalog.Domain.Entities.Category>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once
        );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(ThrowErrorWhenCannotInstantiateCategory))]
    [MemberData(
        nameof(CreateCategoryUsecaseTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateCategoryUsecaseTestDataGenerator))
    ]
    public async void ThrowErrorWhenCannotInstantiateCategory(CreateCategoryInput input)
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var usecase = new CreateCategoryUsecase(repositoryMock.Object, unitOfWorkMock.Object);

        Func<Task> task = async () => await usecase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<EntityValidationException>();
    }
}
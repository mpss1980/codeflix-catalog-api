using Codeflix.Catalog.Application.Usecases.Category.Commons;
using Codeflix.Catalog.Application.Usecases.Category.ListCategories;
using Codeflix.Catalog.Domain.Params;
using FluentAssertions;
using Moq;
using Xunit;
using CategoryDomain = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Usecases.Category.ListCategories;

[Collection(nameof(ListCategoriesUsecaseTestFixture))]
public class ListCategoriesUsecaseTest
{
    private readonly ListCategoriesUsecaseTestFixture _fixture;

    public ListCategoriesUsecaseTest(ListCategoriesUsecaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ShouldListCategories))]
    public async Task ShouldListCategories()
    {
        var repository = _fixture.GetCategoryRepositoryMock();
        var categoryList = _fixture.CreateCategories(10);
        var input = new ListCategoriesInput(
            page: 2,
            perPage: 10,
            search: "search-example",
            sort: "name",
            dir: SearchOrder.Ascending
        );
        var repositoryOutputSearch = new SearchOutput<CategoryDomain.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: _fixture.CreateCategories(10),
            total: 100
        );

        repository.Setup(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositoryOutputSearch);

        var usecase = new ListCategoriesUsecase(repository.Object);

        var output = await usecase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositoryOutputSearch.CurrentPage);
        output.PerPage.Should().Be(repositoryOutputSearch.PerPage);
        output.Total.Should().Be(repositoryOutputSearch.Total);
        output.Items.Should().HaveCount(repositoryOutputSearch.Items.Count);
        ((List<CategoryOutput>)output.Items).ForEach((item) =>
        {
            var categoryFromRepository = repositoryOutputSearch.Items.FirstOrDefault(x => x.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(categoryFromRepository!.Name);
            item.Description.Should().Be(categoryFromRepository.Description);
            item.IsActive.Should().Be(categoryFromRepository.IsActive);
            item.CreatedAt.Should().Be(categoryFromRepository.CreatedAt);
        });

        repository.Verify(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
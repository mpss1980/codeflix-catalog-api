using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EntityFramework.UnitOfWork;
using UnitOfWorkInfra = Codeflix.Catalog.Infra.Data.EntityFramework;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Commit))]
    public async Task Commit()
    {
        var dbContext = _fixture.CreateDbContext();
        var categoryList = _fixture.GetCategoryList();
        await dbContext.AddRangeAsync(categoryList);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);
        
        var assetDbContext = _fixture.CreateDbContext(true);
        var savedCategories = assetDbContext.Categories.AsNoTracking().ToList();
        savedCategories.Should().HaveCount(categoryList.Count);
    }
    
    [Fact(DisplayName = nameof(Rollback))]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        var task =  async () => await unitOfWork.Rollback(CancellationToken.None);
        
       await task.Should().NotThrowAsync();
    }
}
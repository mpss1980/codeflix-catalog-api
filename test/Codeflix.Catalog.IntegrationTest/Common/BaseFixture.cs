using Bogus;
using Codeflix.Catalog.Infra.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.IntegrationTest.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; } = new("pt_BR");
    
    public CodeflixCategoryDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new CodeflixCategoryDbContext(
            new DbContextOptionsBuilder<CodeflixCategoryDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );
        if (!preserveData) dbContext.Database.EnsureDeleted();
        return dbContext;
    }
}
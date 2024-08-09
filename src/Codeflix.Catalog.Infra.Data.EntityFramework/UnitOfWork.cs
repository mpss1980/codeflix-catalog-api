using Codeflix.Catalog.Application.Interfaces;

namespace Codeflix.Catalog.Infra.Data.EntityFramework;

public class UnitOfWork : IUnitOfWork
{
    private readonly CodeflixCategoryDbContext _dbContext;

    public UnitOfWork(CodeflixCategoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
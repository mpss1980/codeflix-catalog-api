using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.Domain.Params;
using Codeflix.Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.Infra.Data.EntityFramework.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CodeflixCategoryDbContext _dbContext;
    private DbSet<Category> Categories => _dbContext.Set<Category>();

    public CategoryRepository(CodeflixCategoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(Category category, CancellationToken cancellationToken)
    {
        await Categories.AddAsync(category, cancellationToken);
    }

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await Categories.FindAsync(new object[] { id }, cancellationToken);
        NotFoundException.ThrowIfNull(category, $"Category {id} not found.");
        return category!;
    }

    public Task Delete(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
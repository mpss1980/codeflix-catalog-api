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
        var category = await Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        NotFoundException.ThrowIfNull(category, $"Category {id} not found.");
        return category!;
    }

    public Task Delete(Category category, CancellationToken _)
    {
        return Task.FromResult(Categories.Remove(category));
    }

    public Task Update(Category category, CancellationToken _)
    {
        return Task.FromResult(Categories.Update(category));
    }

    public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = Categories.AsNoTracking();
        query = AddOrderToQuery(query, input.OrderBy, input.Order);
        
        if (!string.IsNullOrWhiteSpace(input.Search))
        {
            query = query.Where(x => x.Name.Contains(input.Search));
        }

        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken: cancellationToken);
        return new SearchOutput<Category>(input.Page, input.PerPage, total, items);
    }

    private IQueryable<Category> AddOrderToQuery(
        IQueryable<Category> query,
        string orderProperty,
        SearchOrder order
    )
    {
        return (orderProperty, order) switch
        {
            ("name", SearchOrder.Ascending) => query.OrderBy(x => x.Name),
            ("name", SearchOrder.Descending) => query.OrderByDescending(x => x.Name),
            ("id", SearchOrder.Ascending) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Descending) => query.OrderByDescending(x => x.Id),
            ("createdAt", SearchOrder.Ascending) => query.OrderBy(x => x.CreatedAt),
            ("createdAt", SearchOrder.Descending) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name)
        };
    }
}
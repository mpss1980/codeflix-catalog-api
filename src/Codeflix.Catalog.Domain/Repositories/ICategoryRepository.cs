using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.Domain.SeedWork;

namespace Codeflix.Catalog.Domain.Repositories;

public interface ICategoryRepository : IRepository
{
    Task Insert(Category category, CancellationToken cancellationToken);
    Task<Category> Get(Guid id, CancellationToken cancellationToken);
    Task Delete(Category category, CancellationToken cancellationToken);
    Task Update(Category category, CancellationToken cancellationToken);
}
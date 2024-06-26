using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.Domain.SeedWork;

namespace Codeflix.Catalog.Domain.Repositories;

public interface ICategoryRepository : IRepository
{
    Task Insert(Category category, CancellationToken cancellationToken);
}
namespace Codeflix.Catalog.Domain.SeedWork;

public abstract class Entity
{
    public Guid Id { get; } = Guid.NewGuid();
}
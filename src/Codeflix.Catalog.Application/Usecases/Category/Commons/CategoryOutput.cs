namespace Codeflix.Catalog.Application.Usecases.Category.Commons;

using DomainEntity = Domain.Entities;

public class CategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    private CategoryOutput(Guid id, string name, string description, bool isActive, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static CategoryOutput FromCategory(DomainEntity.Category category) => new(
        id: category.Id,
        name: category.Name,
        description: category.Description,
        isActive: category.IsActive,
        createdAt: category.CreatedAt
    );
}
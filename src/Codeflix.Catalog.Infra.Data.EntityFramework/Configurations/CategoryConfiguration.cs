using Codeflix.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codeflix.Catalog.Infra.Data.EntityFramework.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Name)
            .HasMaxLength(255);
        builder.Property(category => category.Description)
            .HasMaxLength(100000);
    }
}
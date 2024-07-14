using Codeflix.Catalog.Domain.Entities;
using Codeflix.Catalog.Infra.Data.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.Infra.Data.EntityFramework;

public class CodeflixCategoryDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();

    public CodeflixCategoryDbContext(DbContextOptions<CodeflixCategoryDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
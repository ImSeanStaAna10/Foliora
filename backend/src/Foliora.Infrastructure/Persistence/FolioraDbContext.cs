using Microsoft.EntityFrameworkCore;
using Foliora.Domain.Entities;

namespace Foliora.Infrastructure.Persistence;

public class FolioraDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public FolioraDbContext(DbContextOptions<FolioraDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}

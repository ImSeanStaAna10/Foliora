using Microsoft.EntityFrameworkCore;
using Foliora.Domain.Entities;

namespace Foliora.Infrastructure.Persistence;

public class FolioraDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<ExternalIdentity> ExternalIdentities => Set<ExternalIdentity>();

    public FolioraDbContext(DbContextOptions<FolioraDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<ExternalIdentity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Provider).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SubjectId).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => new { e.Provider, e.SubjectId }).IsUnique();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.ExternalIdentities)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

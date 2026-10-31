using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Foliora.Infrastructure.Persistence;

public class FolioraDbContextFactory : IDesignTimeDbContextFactory<FolioraDbContext>
{
    public FolioraDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=127.0.0.1;Port=5433;Database=foliora_dev;Username=postgres;Password=postgres";

        var builder = new DbContextOptionsBuilder<FolioraDbContext>();
        builder.UseNpgsql(connectionString);

        return new FolioraDbContext(builder.Options);
    }
}

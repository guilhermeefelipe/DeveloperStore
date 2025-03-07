using DeveloperStore.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Repositories.Repositories;

public interface IDatabaseMigrator
{
    Task MigrateAsync();
}

public class DatabaseMigrator : IDatabaseMigrator
{
    private readonly DeveloperStoreDbContext dbContext;

    public DatabaseMigrator(DeveloperStoreDbContext dbContext)
        => this.dbContext = dbContext;

    public async Task MigrateAsync()
    {
        await dbContext.Database.MigrateAsync();
        await dbContext.SaveChangesAsync();
    }
}

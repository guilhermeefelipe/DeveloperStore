using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Repositories.DbContexts.Base;

public abstract class DbContextBase : DbContext
{
    protected DbContextBase(DbContextOptions options)
        : base(options)
    {
    }

    protected DbContextBase()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EnforceRestrictDeleteBehavior(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void EnforceRestrictDeleteBehavior(ModelBuilder modelBuilder)
    {
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(i => i.GetForeignKeys())
            .Where(i => i.DeleteBehavior != DeleteBehavior.Restrict);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }
}

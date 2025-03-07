using DeveloperStore.Domain.Entities;
using DeveloperStore.Repositories.DbContexts.Base;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Repositories.DbContexts;


public class DeveloperStoreDbContext : DbContextBase
{
    public DeveloperStoreDbContext(DbContextOptions<DeveloperStoreDbContext> options)
        : base(options)
    {
    }

    protected DeveloperStoreDbContext()
    {
    }
    public DbSet<Address> Address { get; set; } = default!;
    public DbSet<Cart> Cart { get; set; } = default!;
    public DbSet<Geolocation> Geolocation { get; set; } = default!;
    public DbSet<Name> Name { get; set; } = default!;
    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Rating> Rating { get; set; } = default!;
    public DbSet<User> User { get; set; } = default!;
}

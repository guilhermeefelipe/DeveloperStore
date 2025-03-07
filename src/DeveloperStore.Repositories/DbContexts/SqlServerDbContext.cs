using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DeveloperStore.Repositories.DbContexts;

public class SqlServerDbContext : DeveloperStoreDbContext
{
    private readonly IConfiguration configuration;

    public SqlServerDbContext(IConfiguration configuration)
        => this.configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = configuration["ConnectionString"];
        optionsBuilder.UseSqlServer(connectionString);
    }
}
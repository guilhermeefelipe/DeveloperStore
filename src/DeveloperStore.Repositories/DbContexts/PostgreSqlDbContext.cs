using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DeveloperStore.Repositories.DbContexts;

public class PostgreSqlDbContext : DeveloperStoreDbContext
{
    private readonly IConfiguration configuration;

    public PostgreSqlDbContext(IConfiguration configuration)
        => this.configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = configuration["ConnectionString"];
        optionsBuilder.UseNpgsql(connectionString);
    }
}

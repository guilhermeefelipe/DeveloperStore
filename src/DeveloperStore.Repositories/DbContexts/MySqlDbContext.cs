using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeveloperStore.Repositories.DbContexts;

public class MySqlDbContext : DeveloperStoreDbContext
{
    private readonly IConfiguration configuration;

    public MySqlDbContext(IConfiguration configuration)
        => this.configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = configuration["ConnectionString"];
        var versionText = configuration["MySqlVersion"];
        var version = ServerVersion.Parse(versionText);
        optionsBuilder.UseMySql(connectionString, version);
    }
}
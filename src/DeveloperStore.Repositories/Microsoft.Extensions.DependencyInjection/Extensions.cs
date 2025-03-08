using DeveloperStore.Repositories;
using DeveloperStore.Repositories.Addresses;
using DeveloperStore.Repositories.DbContexts;
using DeveloperStore.Repositories.Geolocations;
using DeveloperStore.Repositories.Names;
using DeveloperStore.Repositories.Repositories;
using DeveloperStore.Repositories.Users;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static void AddDeveloperStoreRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["Provider"];
        switch (provider)
        {
            case "PostgreSql":
                services.AddDbContext<DeveloperStoreDbContext, PostgreSqlDbContext>();
                break;
            case "SqlServer":
                services.AddDbContext<DeveloperStoreDbContext, SqlServerDbContext>();
                break;
            case "MySql":
                services.AddDbContext<DeveloperStoreDbContext, MySqlDbContext>();
                break;
            default:
                throw new InvalidOperationException($"Provedor de banco de dados '{provider}' não suportado.");
        }

        services.AddDbContext<SqlServerDbContext>();
        services.AddDbContext<MySqlDbContext>();
        services.AddDbContext<PostgreSqlDbContext>();

        services.AddScoped<IDatabaseMigrator, DatabaseMigrator>();
        services.AddScoped(typeof(IExtendedContext<>), typeof(ExtendedContext<>));

        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IAddressesRepository, AddressesRepository>();
        services.AddScoped<IGeolocationsRepository, GeolocationsRepository>();
        services.AddScoped<INamesRepository, NamesRepository>();

    }
}

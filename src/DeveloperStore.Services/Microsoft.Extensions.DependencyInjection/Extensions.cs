using DeveloperStore.Repositories.Repositories;
using DeveloperStore.Services.Addresses;
using DeveloperStore.Services.Carts;
using DeveloperStore.Services.Geolocations;
using DeveloperStore.Services.Names;
using DeveloperStore.Services.Users;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddDeveloperStoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDeveloperStoreRepositories(configuration);

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IAddressesService, AddressesService>();
        services.AddScoped<IGeolocationsService, GeolocationsService>();
        services.AddScoped<INamesService, NamesService>();

        return services;
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var migrator = scope.ServiceProvider.GetRequiredService<IDatabaseMigrator>();
        await migrator.MigrateAsync();
    }
}

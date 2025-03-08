using DeveloperStore.Domain.Dto.Geolocation;
using DeveloperStore.Repositories.Geolocations;

namespace DeveloperStore.Services.Geolocations;

public interface IGeolocationsService
{
    Task<int> CreateAsync(GeolocationCreateEditDto model);
}

public class GeolocationsService : IGeolocationsService
{
    private readonly IGeolocationsRepository GeolocationssRepository;

    public GeolocationsService(IGeolocationsRepository GeolocationssRepository)
    {
        this.GeolocationssRepository = GeolocationssRepository;
    }

    public async Task<int> CreateAsync(GeolocationCreateEditDto model)
        => await GeolocationssRepository.CreateAsync(model);
}
using DeveloperStore.Domain.Dto.Address;
using DeveloperStore.Domain.Dto.Geolocation;
using DeveloperStore.Repositories.Addresses;
using DeveloperStore.Repositories.Geolocations;

namespace DeveloperStore.Services.Geolocations;

public interface IGeolocationsService
{
    Task<int> CreateAsync(GeolocationDto model);
    Task UpdateAsync(int id, GeolocationDto model);
}

public class GeolocationsService : IGeolocationsService
{
    private readonly IGeolocationsRepository geolocationssRepository;

    public GeolocationsService(IGeolocationsRepository geolocationssRepository)
    {
        this.geolocationssRepository = geolocationssRepository;
    }

    public async Task<int> CreateAsync(GeolocationDto model)
        => await geolocationssRepository.CreateAsync(model);
    public async Task UpdateAsync(int id, GeolocationDto model)
    => await geolocationssRepository.UpdateAsync(id, model);
}
using DeveloperStore.Domain.Dto.Rating;
using DeveloperStore.Repositories.Raties;

namespace DeveloperStore.Services.Raties;

public interface IRatiesService
{
    Task<int> CreateAsync(RatingDto model);
    Task UpdateAsync(int id, RatingDto model);
}

public class RatiesService : IRatiesService
{
    private readonly IRatiesRepository ratiesRepository;

    public RatiesService(IRatiesRepository ratiesRepository)
    {
        this.ratiesRepository = ratiesRepository;
    }

    public async Task<int> CreateAsync(RatingDto model)
        => await ratiesRepository.CreateAsync(model);
    public async Task UpdateAsync(int id, RatingDto model)
    => await ratiesRepository.UpdateAsync(id, model);
}
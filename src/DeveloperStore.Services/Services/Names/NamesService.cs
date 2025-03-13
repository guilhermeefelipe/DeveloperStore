using DeveloperStore.Domain.Dto.Name;
using DeveloperStore.Repositories.Names;

namespace DeveloperStore.Services.Names;

public interface INamesService
{
    Task<int> CreateAsync(NameDto model);
    Task UpdateAsync(int id, NameDto model);
}

public class NamesService : INamesService
{
    private readonly INamesRepository namesRepository;

    public NamesService(INamesRepository namesRepository)
    {
        this.namesRepository = namesRepository;
    }

    public async Task<int> CreateAsync(NameDto model)
        => await namesRepository.CreateAsync(model);

    public async Task UpdateAsync(int id, NameDto model)
    => await namesRepository.UpdateAsync(id, model);
}
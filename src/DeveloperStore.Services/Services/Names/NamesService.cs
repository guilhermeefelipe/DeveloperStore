using DeveloperStore.Domain.Dto.Name;
using DeveloperStore.Domain.Dto.Users;
using DeveloperStore.Repositories.Names;

namespace DeveloperStore.Services.Names;

public interface INamesService
{
    Task<int> CreateAsync(NameCreateEditDto model);
}

public class NamesService : INamesService
{
    private readonly INamesRepository NamesRepository;

    public NamesService(INamesRepository NamesRepository)
    {
        this.NamesRepository = NamesRepository;
    }

    public async Task<int> CreateAsync(NameCreateEditDto model)
        => await NamesRepository.CreateAsync(model);
}
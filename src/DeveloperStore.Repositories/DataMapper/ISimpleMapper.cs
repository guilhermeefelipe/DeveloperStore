namespace DeveloperStore.Repositories.DataMapper;

public interface ISimpleMapper
{
    IEnumerable<string> Map<T, U>(T source, U target)
        where T : class
        where U : class;
}

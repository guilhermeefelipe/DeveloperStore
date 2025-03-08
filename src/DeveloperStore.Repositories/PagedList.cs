namespace DeveloperStore;

public interface IPagedList<T>
{
    IReadOnlyList<T> Items { get; }
    int CurrentPage { get; }
    int TotalPages { get; }
    int TotalItems { get; }
}

public class PagedList<T> : IPagedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalItems { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }

    public PagedList(int page, int pageSize, int totalCount, IReadOnlyList<T> items)
    {
        CurrentPage = page;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalItems = totalCount;
        Items = items;
    }
}
